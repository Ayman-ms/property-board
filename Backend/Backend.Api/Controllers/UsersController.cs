using Microsoft.AspNetCore.Mvc;
using Backend.Api.Repository;
using Backend.Api.Common;
using Backend.Api.DTOs.User;
using Backend.Api.DTOs.Common;
using Backend.Api.Models;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResultDto<UserDto>>>> GetUsers(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var users = await _userRepository.GetUsersPaginatedAsync(pagination);
                
                var userDtos = users.Data.Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone,
                    UserType = u.UserType,
                    Language = u.Language,
                    IsActive = u.IsActive,
                    IsVerified = u.IsVerified,
                    CreatedAt = u.CreatedAt ?? DateTime.UtcNow
                });

                var result = new PaginatedResultDto<UserDto>
                {
                    Data = userDtos.ToList(),
                    TotalCount = users.TotalCount,
                    Page = users.Page,
                    PageSize = users.PageSize,
                    TotalPages = users.TotalPages
                };

                return Ok(ApiResponse<PaginatedResultDto<UserDto>>.Success(result, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PaginatedResultDto<UserDto>>.Failure(
                    "Error retrieving users", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                
                if (user == null)
                {
                    return NotFound(ApiResponse<UserDto>.Failure("User not found"));
                }

                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    UserType = user.UserType,
                    Language = user.Language,
                    IsActive = user.IsActive,
                    IsVerified = user.IsVerified,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                };

                return Ok(ApiResponse<UserDto>.Success(userDto, "User retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDto>.Failure(
                    "Error retrieving user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<UserDto>.Failure("Validation failed", errors));
                }

                // التحقق من وجود البريد الإلكتروني
                var emailExists = await _userRepository.EmailExistsAsync(createUserDto.Email);
                if (emailExists)
                {
                    return BadRequest(ApiResponse<UserDto>.Failure("Email already exists"));
                }

                var user = new User
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Email = createUserDto.Email,
                    Phone = createUserDto.Phone,
                    Language = createUserDto.Language,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                    IsActive = true,
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.AddAsync(user);

                var userDto = new UserDto
                {
                    UserId = createdUser.UserId,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    Phone = createdUser.Phone,
                    Language = createdUser.Language,
                    IsActive = createdUser.IsActive,
                    IsVerified = createdUser.IsVerified,
                    CreatedAt = createdUser.CreatedAt ?? DateTime.UtcNow                    
                };

                return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, 
                    ApiResponse<UserDto>.Success(userDto, "User created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDto>.Failure(
                    "Error creating user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<UserDto>.Failure("Validation failed", errors));
                }

                var existingUser = await _userRepository.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(ApiResponse<UserDto>.Failure("User not found"));
                }

                // التحقق من البريد الإلكتروني إذا تم تغييره
                if (updateUserDto.Email != existingUser.Email)
                {
                    var emailExists = await _userRepository.EmailExistsAsync(updateUserDto.Email);
                    if (emailExists)
                    {
                        return BadRequest(ApiResponse<UserDto>.Failure("Email already exists"));
                    }
                    existingUser.Email = updateUserDto.Email;
                }

                // تحديث البيانات
                existingUser.FirstName = updateUserDto.FirstName;
                existingUser.LastName = updateUserDto.LastName;
                existingUser.Phone = updateUserDto.Phone;

                existingUser.Language = updateUserDto.Language;

                var updatedUser = await _userRepository.UpdateAsync(existingUser);

                var userDto = new UserDto
                {
                    UserId = updatedUser.UserId,
                    FirstName = updatedUser.FirstName,
                    LastName = updatedUser.LastName,
                    Email = updatedUser.Email,
                    Phone = updatedUser.Phone,
                    Language = updatedUser.Language,
                    IsActive = updatedUser.IsActive,
                    IsVerified = updatedUser.IsVerified,
                    CreatedAt = updatedUser.CreatedAt ?? DateTime.UtcNow
                };

                return Ok(ApiResponse<UserDto>.Success(userDto, "User updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDto>.Failure(
                    "Error updating user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int id)
        {
            try
            {
                var userExists = await _userRepository.ExistsAsync(id);
                if (!userExists)
                {
                    return NotFound(ApiResponse<object>.Failure("User not found"));
                }

                var deleted = await _userRepository.DeleteAsync(id);
                
                if (deleted)
                {
                    return Ok(ApiResponse<object>.Success(null, "User deleted successfully"));
                }
                else
                {
                    return StatusCode(500, ApiResponse<object>.Failure("Failed to delete user"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure(
                    "Error deleting user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                
                if (user == null)
                {
                    return NotFound(ApiResponse<UserDto>.Failure("User not found"));
                }

                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Language = user.Language,
                    IsActive = user.IsActive,
                    IsVerified = user.IsVerified,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                };

                return Ok(ApiResponse<UserDto>.Success(userDto, "User retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDto>.Failure(
                    "Error retrieving user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<ActionResult<ApiResponse<object>>> ActivateUser(int id)
        {
            try
            {
                var result = await _userRepository.ActivateUserAsync(id);
                
                if (result)
                {
                    return Ok(ApiResponse<object>.Success(null, "User activated successfully"));
                }
                else
                {
                    return NotFound(ApiResponse<object>.Failure("User not found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure(
                    "Error activating user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult<ApiResponse<object>>> DeactivateUser(int id)
        {
            try
            {
                var result = await _userRepository.DeactivateUserAsync(id);
                
                if (result)
                {
                    return Ok(ApiResponse<object>.Success(null, "User deactivated successfully"));
                }
                else
                {
                    return NotFound(ApiResponse<object>.Failure("User not found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure(
                    "Error deactivating user", 
                    new List<string> { ex.Message }));
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetActiveUsers()
        {
            try
            {
                var users = await _userRepository.GetActiveUsersAsync();
                
                var userDtos = users.Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Language = u.Language,
                    IsActive = u.IsActive,
                    IsVerified = u.IsVerified,
                    CreatedAt = u.CreatedAt ?? DateTime.UtcNow
                });

                return Ok(ApiResponse<IEnumerable<UserDto>>.Success(userDtos, "Active users retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserDto>>.Failure(
                    "Error retrieving active users", 
                    new List<string> { ex.Message }));
            }
        }
    }
}