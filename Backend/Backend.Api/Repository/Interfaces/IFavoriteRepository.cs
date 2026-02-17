using Backend.Api.Models;

namespace Backend.Api.Repository
{
    public interface IFavoriteRepository
    {
        Task<bool> AddFavoriteAsync(int userId, int propertyId);
        Task<bool> RemoveFavoriteAsync(int userId, int propertyId);
        Task<IEnumerable<object>> GetUserFavoritesAsync(int userId); 
        Task<bool> IsFavoriteAsync(int userId, int propertyId);
    }
}