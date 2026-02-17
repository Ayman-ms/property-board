using Backend.Api.Data;
using Backend.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFavoriteAsync(int userId, int propertyId)
        {
            // التحقق من عدم وجوده مسبقاً
            if (await IsFavoriteAsync(userId, propertyId)) return true;

            var favorite = new Favorite 
            { 
                UserId = userId, 
                PropertyId = propertyId,
                CreatedAt = DateTime.UtcNow 
            };

            _context.Favorites.Add(favorite);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsFavoriteAsync(int userId, int propertyId)
        {
            return await _context.Favorites.AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int propertyId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
            
            if (favorite == null) return false;

            _context.Favorites.Remove(favorite);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<object>> GetUserFavoritesAsync(int userId)
        {
            // الربط بين جدول المفضلة وجدول العقارات حسب الـ Diagram
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Join(_context.Properties,
                    f => f.PropertyId,
                    p => p.PropertyId,
                    (f, p) => new { 
                        f.FavoriteId, 
                        p.PropertyId, 
                        p.Title, // مطابق للـ Diagram
                        p.Price, // مطابق للـ Diagram
                        p.City,  // مطابق للـ Diagram
                        p.CreatedAt
                    })
                .ToListAsync();
        }
    }
}