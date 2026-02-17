using Backend.Api.Models;
using Backend.Api.DTOs.Common;

namespace Backend.Api.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task<PaginatedResultDto<User>> GetUsersPaginatedAsync(PaginationDto pagination);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);
        new Task<User> UpdateAsync(User user);
    }
}