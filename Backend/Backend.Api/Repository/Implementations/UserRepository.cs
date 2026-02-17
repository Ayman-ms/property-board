using Backend.Api.Data;
using Backend.Api.Models;
using Backend.Api.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user != null && user.IsActive)
            {
                // مؤقتاً نقارن مباشرة - يجب تشفير كلمة المرور لاحقاً
                if (user.PasswordHash == password)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<PaginatedResultDto<User>> GetUsersPaginatedAsync(PaginationDto pagination)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResultDto<User>
            {
                Data = users,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow; // ✅ الآن يعمل بعد إضافة الخاصية
                await UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                await UpdateAsync(user);
                return true;
            }
            return false;
        }

        public new async Task <User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}