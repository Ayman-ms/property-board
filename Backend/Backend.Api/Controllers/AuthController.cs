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

        // ==========================================
        // LOGIN
        // ==========================================
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(ApiResponse<object>.Failure("Invalid email or password"));
            }

            var token = GenerateJwtToken(user);

            var result = new
            {
                token = token,
                user = new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Phone,
                    user.UserType,
                    user.ProfileImageUrl
                }
            };

            return Ok(ApiResponse<object>.Success(result, "Login successful"));
        }

        // ==========================================
        // REGISTER
        // ==========================================
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

        // ==========================================
        // EXTERNAL LOGIN (Google / Facebook)
        // ==========================================
        [HttpPost("external")]
        public async Task<ActionResult<ApiResponse<object>>> ExternalLogin([FromBody] ExternalLoginDto dto)
        {
            var decodedToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.IdToken);
            var email = decodedToken.Claims["email"].ToString();

            var user = await _userRepository.GetByEmailAsync(email!);

            if (user == null)
            {
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

            var myToken = GenerateJwtToken(user);

            var result = new
            {
                token = myToken,
                user = new
                {
                    user.UserId,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.UserType,
                    user.ProfileImageUrl
                }
            };

            return Ok(ApiResponse<object>.Success(result, "External Authentication Successful"));
        }

        // ==========================================
        // FORGOT PASSWORD
        // ==========================================
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<object>>> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return Ok(ApiResponse<object>.Success(null!, "Link generated"));

            var resetToken = Guid.NewGuid().ToString();

            user.PasswordResetToken = resetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1).ToString();
            await _userRepository.UpdateAsync(user);

            var resetLink = $"http://localhost:4200/auth/reset-password?token={resetToken}&email={user.Email}";

            Console.WriteLine("***********************************************");
            Console.WriteLine("RESET LINK: " + resetLink);
            Console.WriteLine("***********************************************");

            return Ok(ApiResponse<object>.Success(null!, "Reset link sent to your email."));
        }

        // ==========================================
        // RESET PASSWORD
        // ==========================================
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordResetToken != dto.Token || DateTime.Parse(user.ResetTokenExpires!) < DateTime.UtcNow)
            {
                return BadRequest(ApiResponse<object>.Failure("Invalid or expired token."));
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _userRepository.UpdateAsync(user);

            return Ok(ApiResponse<object>.Success(null!, "Password has been reset successfully."));
        }

        // ==========================================
        // PRIVATE HELPERS
        // ==========================================
        private string GenerateJwtToken(Models.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim(ClaimTypes.Role, user.UserType),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
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
