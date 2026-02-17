using Backend.Api.DTOs;
using Backend.Api.Services.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactMessage([FromBody] ContactDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ضع إيميلك الشخصي هنا لاستقبال الرسائل
                string myAdminEmail = "mohamad513@gmail.com"; 

                string emailSubject = $"[Contact Form]: {dto.Subject}";
                string emailBody = $@"
                    <h3>New message from your website</h3>
                    <p><b>From:</b> {dto.Name}</p>
                    <p><b>Email:</b> {dto.Email}</p>
                    <p><b>Message:</b></p>
                    <p>{dto.Message}</p>";

                await _emailService.SendEmailAsync(myAdminEmail, emailSubject, emailBody);

                return Ok(new { message = "Sent Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}