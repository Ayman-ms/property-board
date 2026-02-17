using Backend.Api.Data;
using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Property;
using Backend.Api.Models;
using Backend.Api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Repository.Implementations
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context) { }

        // جلب العقار مع صوره (PropertyMedia)
        public async Task<Property?> GetPropertyWithDetailsAsync(int propertyId)
        {
            return await _context.Properties
                .Include(p => p.PropertyMedia) // يتطلب تعريف ICollection<PropertyMedia> في موديل Property
                .FirstOrDefaultAsync(p => p.PropertyId == propertyId);
        }

        public async Task<PaginatedResultDto<Property>> GetPropertiesPaginatedAsync(PaginationDto pagination)
        {
            var query = _context.Properties.AsQueryable();
            var totalCount = await query.CountAsync();

            var data = await query
                .Include(p => p.PropertyMedia.Where(m => m.IsPrimary == true)) // جلب الصورة الأساسية فقط للسرعة
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResultDto<Property>
            {
                Data = data,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PaginatedResultDto<Property>> SearchPropertiesAsync(PropertySearchDto search)
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search.City))
                query = query.Where(p => p.City.Contains(search.City));

            if (search.MinPrice.HasValue)
                query = query.Where(p => p.Price >= search.MinPrice);

            if (search.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= search.MaxPrice);

            // تم التعديل ليتوافق مع اسم الحقل في الـ Diagram (user_id)
            if (!string.IsNullOrWhiteSpace(search.ListingType))
                query = query.Where(p => p.ListingType == search.ListingType);

            var totalCount = await query.CountAsync();
            int page = search.Page > 0 ? search.Page : 1;
            int pageSize = search.PageSize > 0 ? search.PageSize : 10;

            var data = await query
                .Include(p => p.PropertyMedia)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<Property>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int userId)
        {
            return await _context.Properties
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetFeaturedPropertiesAsync(int count)
        {
            return await _context.Properties
                .Include(p => p.PropertyMedia.Where(m => m.IsPrimary == true))
                .OrderByDescending(p => p.ViewsCount) // تم اختيار العقارات الأكثر مشاهدة كمميزة
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> UpdateViewCountAsync(int propertyId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return false;

            property.ViewsCount = (property.ViewsCount ?? 0) + 1;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetAverageRatingAsync(int propertyId)
        {
            var ratings = await _context.PropertyReviews
                .Where(r => r.PropertyId == propertyId)
                .Select(r => r.Rating)
                .ToListAsync();

            return ratings.Any() ? (decimal)ratings.Average() : 0;
        }

        // إضافات الـ Interface الناقصة
        public override async Task<IEnumerable<Property>> GetAllAsync() => await _context.Properties.ToListAsync();

        public override async Task<Property?> GetByIdAsync(int id) => await _context.Properties.FindAsync(id);

        public override async Task<Property> AddAsync(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task AddMediaAsync(PropertyMedia media)
        {
            _context.PropertyMedia.Add(media); // تأكد من اسم الـ DbSet في الـ Context
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PropertyMedia>> GetMediaByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyMedia
                .Where(m => m.PropertyId == propertyId)
                .ToListAsync();
        }
    }
}