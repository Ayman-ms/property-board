using Microsoft.AspNetCore.Mvc;
using Backend.Api.Common;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImagesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload")]
        // [Authorize] // يمكنك تفعيلها إذا كنت تريد منع غير المسجلين من رفع الصور
        public async Task<ActionResult<ApiResponse<string>>> UploadImage(IFormFile file)
        {
            try
            {
                // 1. التحقق من وجود الملف
                if (file == null || file.Length == 0)
                    return BadRequest(ApiResponse<string>.Failure("لم يتم اختيار ملف"));

                // 2. التحقق من امتداد الملف (للأمان)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    return BadRequest(ApiResponse<string>.Failure("صيغة الملف غير مدعومة. استخدم JPG أو PNG"));

                // 3. تحديد مسار الحفظ
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                // 4. توليد اسم فريد للملف لمنع التكرار
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // 5. حفظ الملف فعلياً
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 6. إرجاع رابط الصورة الذي سيخزن في قاعدة البيانات
                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                
                return Ok(ApiResponse<string>.Success(fileUrl, "تم رفع الصورة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"خطأ داخلي: {ex.Message}"));
            }
        }
    }
}