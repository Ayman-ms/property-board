using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Backend.Api.Data;
using Backend.Api.Models;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize] // يجب أن يكون المستخدم مسجلاً للدخول لرؤية رسائله
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. جلب كل المحادثات الخاصة بالمستخدم (الـ Inbox)
        // ==========================================
        [HttpGet("conversations")]
        public async Task<IActionResult> GetMyConversations()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var conversations = await _context.Conversations
                .Where(c => c.BuyerUserId == userId || c.SellerUserId == userId)
                .OrderByDescending(c => c.UpdatedAt)
                .Select(c => new
                {
                    c.ConversationId,
                    c.PropertyId,
                    // تحديد الطرف الآخر في المحادثة لعرض اسمه لاحقاً
                    OtherUserId = c.BuyerUserId == userId ? c.SellerUserId : c.BuyerUserId,
                    LastMessage = _context.Messages
                        .Where(m => m.ConversationId == c.ConversationId)
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => m.MessageText)
                        .FirstOrDefault(),
                    c.UpdatedAt,
                    UnreadCount = _context.Messages
                        .Count(m => m.ConversationId == c.ConversationId && m.SenderUserId != userId && !m.IsRead)
                })
                .ToListAsync();

            return Ok(conversations);
        }

        // ==========================================
        // 2. جلب الرسائل داخل محادثة معينة (عند النقر عليها)
        // ==========================================
        [HttpGet("conversations/{id}/messages")]
        public async Task<IActionResult> GetChatMessages(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // التحقق من أن المستخدم طرف في هذه المحادثة
            var conversation = await _context.Conversations
                .AnyAsync(c => c.ConversationId == id && (c.BuyerUserId == userId || c.SellerUserId == userId));

            if (!conversation) return Forbid();

            var messages = await _context.Messages
                .Where(m => m.ConversationId == id)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            // تحديث الرسائل لتصبح "مقروءة" بمجرد فتح المحادثة
            var unreadMessages = messages.Where(m => m.SenderUserId != userId && !m.IsRead).ToList();
            if (unreadMessages.Any())
            {
                unreadMessages.ForEach(m => m.IsRead = true);
                await _context.SaveChangesAsync();
            }

            return Ok(messages);
        }

        // ==========================================
        // 3. إرسال رسالة جديدة
        // ==========================================
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // 1. إيجاد أو إنشاء محادثة
            var conv = await _context.Conversations.FindAsync(dto.ConversationId);
            if (conv == null) return NotFound("The conversation does not exist.");

            // 2. إضافة الرسالة
            var newMessage = new Message
            {
                ConversationId = dto.ConversationId,
                SenderUserId = senderId,
                MessageText = dto.MessageText,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Messages.Add(newMessage);

            // 3. تحديث وقت المحادثة
            conv.UpdatedAt = DateTime.UtcNow;

            // 4. إنشاء إشعار للطرف الآخر
            var receiverId = conv.BuyerUserId == senderId ? conv.SellerUserId : conv.BuyerUserId;
            var notification = new Notification
            {
                UserId = receiverId,
                Type = "NewMessage",
                Title = "New Message",
                Message = $"You have a new message regarding property #{conv.PropertyId}",
                RelatedId = conv.ConversationId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
            return Ok(newMessage);
        }

        // أضف هذه الدالة داخل ChatController
[HttpPost("start")]
public async Task<IActionResult> StartOrContinueConversation([FromBody] StartConversationDto dto)
{
    var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

    // 1. البحث عن المحادثة
    var conversation = await _context.Conversations
        .FirstOrDefaultAsync(c => c.PropertyId == dto.PropertyId && 
                                 ((c.BuyerUserId == currentUserId && c.SellerUserId == dto.SellerId) || 
                                  (c.BuyerUserId == dto.SellerId && c.SellerUserId == currentUserId)));

    if (conversation == null)
    {
        // إنشاء محادثة جديدة تماماً
        conversation = new Conversation
        {
            PropertyId = dto.PropertyId,
            BuyerUserId = currentUserId,
            SellerUserId = dto.SellerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(); // حفظ أولاً للحصول على ConversationId
    }
    else 
    {
        // تحديث المحادثة الموجودة
        conversation.UpdatedAt = DateTime.UtcNow;
        _context.Conversations.Update(conversation); // تأكيد التحديث
    }

    // 2. إضافة الرسالة (هذا هو الجزء الذي كان يفشل)
    var newMessage = new Message
    {
        ConversationId = conversation.ConversationId,
        SenderUserId = currentUserId,
        MessageText = dto.MessageText,
        CreatedAt = DateTime.UtcNow,
        IsRead = false
    };

    try 
    {
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync(); // الحفظ النهائي
        return Ok(new { success = true, msg = "Message Saved!" });
    }
    catch (Exception ex) 
    {
        // إذا فشل الحفظ، سيظهر لك السبب الحقيقي هنا في الـ Swagger أو الـ Console
        return BadRequest(new { error = ex.Message, inner = ex.InnerException?.Message });
    }
}


   public class StartConversationDto
        {
            public int PropertyId { get; set; }
            public int SellerId { get; set; }
            public string MessageText { get; set; }
        }
    }

    public class SendMessageDto
    {
        public int ConversationId { get; set; }
        public string MessageText { get; set; } = string.Empty;
    }
}