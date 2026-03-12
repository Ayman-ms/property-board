using Microsoft.AspNetCore.Mvc;
using Backend.Api.Repository;
using Backend.Api.Common;
using Backend.Api.DTOs.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(ApiResponse<object>.Failure("Invalid email or password"));
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            var result = new
            {
                token = token,
                user = new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email
                }
            };

            return Ok(ApiResponse<object>.Success(result, "Login successful"));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register([FromForm] RegisterDto registerDto)
        {
            string? imagePath = null;
            if (registerDto.ProfileImage != null && registerDto.ProfileImage.Length > 0)
            {
                imagePath = await SaveImage(registerDto.ProfileImage);
            }

            var user = new Models.User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                ProfileImageUrl = imagePath,
                UserType = "User",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return Ok(ApiResponse<object>.Success(null!, "Registration successful!"));
        }

        [HttpPost("external")]
        public async Task<ActionResult<ApiResponse<object>>> ExternalLogin([FromBody] ExternalLoginDto dto)
        {
            // 1. for Firebase Authentication, we need to verify the ID token sent by the client (which was obtained from Google Sign-In)
            var decodedToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.IdToken);
            var email = decodedToken.Claims["email"].ToString();

            // 2. search for a user with this email in our database
            var user = await _userRepository.GetByEmailAsync(email!);

            if (user == null)
            {
                // 3. If not found (Register process), create a new account immediately
                user = new Models.User
                {
                    Email = email!,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PasswordHash = "EXTERNAL_ACCOUNT",
                    CreatedAt = DateTime.UtcNow,
                    Provider = dto.Provider
                };
                await _userRepository.AddAsync(user);
            }

            // 4. Generate JWT token for the user
            var myToken = GenerateJwtToken(user);

            var result = new
            {
                token = myToken,
                user = new { user.UserId, user.Email, user.FirstName }
            };

            return Ok(ApiResponse<object>.Success(result, "External Authentication Successful"));
        }
        private string GenerateJwtToken(Models.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FirstName", user.FirstName)
                // User roles can be added here in the future.
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token validity period
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/uploads/profiles/{fileName}";
        }

        // Endpoints for forgot password and reset password

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<object>>> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return Ok(ApiResponse<object>.Success(null!, "Link generated"));

            // create a unique token for password reset (you can use JWT logic here or a simple Guid)
            // Guid is simpler for this use case since we just need to verify it against the database and it doesn't need to carry any claims.
            var resetToken = Guid.NewGuid().ToString();

            // save the token and its expiration time in the database
            user.PasswordResetToken = resetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1).ToString(); // token valid for 1 hour
            await _userRepository.UpdateAsync(user);

            //  Send the E-Mail
            var resetLink = $"http://localhost:4200/auth/reset-password?token={resetToken}&email={user.Email}";

            // Print the link in the black screen (Terminal) so we can view and copy it.
            Console.WriteLine("***********************************************");
            Console.WriteLine("RESET LINK: " + resetLink);
            Console.WriteLine("***********************************************");

            // Leave the send line as a comment for now to avoid an SMTP error.
            // await SendEmailAsync(user.Email, "Reset Your Password", $"Click here to reset: {resetLink}");

            return Ok(ApiResponse<object>.Success(null!, "Reset link sent to your email."));
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordResetToken != dto.Token || DateTime.Parse(user.ResetTokenExpires!) < DateTime.UtcNow)
            {
                return BadRequest(ApiResponse<object>.Failure("Invalid or expired token."));
            }

            // change the password and clear the reset token
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _userRepository.UpdateAsync(user);

            return Ok(ApiResponse<object>.Success(null!, "Password has been reset successfully."));
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var mimeMessage = new MimeKit.MimeMessage();
            mimeMessage.From.Add(new MimeKit.MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
            mimeMessage.To.Add(new MimeKit.MailboxAddress("", email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new MimeKit.TextPart("html") { Text = message };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]!), MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["Password"]);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}