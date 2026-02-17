using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Backend.Api.Data;
using Backend.Api.Models;
using Backend.Api.DTOs.Property;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/property")]
    public class PropertyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            // سحب البيانات من الـ DbContext
            var types = await _context.PropertyTypes
                .Select(t => new
                {
                    t.TypeId,
                    t.TypeName
                })
                .ToListAsync();

            return Ok(types);
        }
        // ==========================================
        // 1. GET ALL
        // ==========================================
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll( string? listingType, string? search, int page = 1, int pageSize = 10)
        {
            var query = _context.Properties
                .Include(p => p.PropertyMedia)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Title.Contains(search) || p.City.Contains(search));

            if (!string.IsNullOrEmpty(listingType))
            {
                // تأكد أن الاسم يطابق الموجود في قاعدة البيانات (Rent أو Sale)
                query = query.Where(p => p.ListingType == listingType);
            }
            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PropertyListDto
                {
                    PropertyId = p.PropertyId,
                    UserId = p.UserId,
                    PropertyTypeId = p.PropertyTypeId ?? 0, // حل خطأ التحويل
                    Title = p.Title,
                    Description = p.Description,
                    ShortDescription = p.ShortDescription,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    Price = p.Price,
                    Currency = p.Currency,
                    IsNegotiable = p.IsNegotiable ?? false,
                    City = p.City,
                    Bedrooms = p.Bedrooms ?? 0,
                    Bathrooms = p.Bathrooms ?? 0,
                    AreaSqm = p.AreaSqm,
                    FloorNumber = p.FloorNumber,
                    TotalFloors = p.TotalFloors,
                    YearBuilt = p.YearBuilt,
                    HasParking = p.HasParking,
                    HasBalcony = p.HasBalcony,
                    HasGarden = p.HasGarden,
                    HasElevator = p.HasElevator,
                    Street = p.Street,
                    PostCode = p.PostCode,
                    Country = p.Country,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    ViewsCount = p.ViewsCount ?? 0,
                    FavoritesCount = p.FavoritesCount ?? 0,
                    InquiriesCount = p.InquiriesCount ?? 0,
                    CreatedAt = p.CreatedAt.GetValueOrDefault(),
                    UpdatedAt = p.UpdatedAt.GetValueOrDefault(),
                    PublishedAt = p.PublishedAt.GetValueOrDefault(),
                    ExpiresAt = p.ExpiresAt.GetValueOrDefault(),
                    MainImageUrl = p.PropertyMedia
                        .OrderBy(m => m.MediaId) // تم التعديل لضمان الترتيب في حال غياب DisplayOrder
                        .Select(m => m.FileUrl)
                        .FirstOrDefault()
                })
                .ToListAsync();

            Response.Headers.Append("X-Total-Count", total.ToString());
            return Ok(data);
        }

        // ==========================================
        // 2. GET BY ID
        // ==========================================
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyMedia)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
                return NotFound("Property not found");

            var dto = new PropertyListDto
            {
                PropertyId = property.PropertyId,
                UserId = property.UserId,
                PropertyTypeId = property.PropertyTypeId ?? 0, // حل خطأ التحويل
                Title = property.Title,
                Description = property.Description,
                ShortDescription = property.ShortDescription,
                ListingType = property.ListingType,
                Status = property.Status,
                Price = property.Price,
                Currency = property.Currency,
                IsNegotiable = property.IsNegotiable ?? false,
                City = property.City,
                Bedrooms = property.Bedrooms ?? 0,
                Bathrooms = property.Bathrooms ?? 0,
                AreaSqm = property.AreaSqm,
                FloorNumber = property.FloorNumber,
                TotalFloors = property.TotalFloors,
                YearBuilt = property.YearBuilt,
                HasParking = property.HasParking,
                HasBalcony = property.HasBalcony,
                HasGarden = property.HasGarden,
                HasElevator = property.HasElevator,
                Street = property.Street,
                PostCode = property.PostCode,
                Country = property.Country,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                ViewsCount = property.ViewsCount ?? 0,
                CreatedAt = property.CreatedAt.GetValueOrDefault(),
                MainImageUrl = property.PropertyMedia.Select(m => m.FileUrl).FirstOrDefault()
            };

            return Ok(dto);
        }

        // ==========================================
        // 3. POST (إنشاء)
        // ==========================================
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePropertyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int finalUserId = string.IsNullOrEmpty(userIdClaim) ? 11 : int.Parse(userIdClaim);

            var property = new Property
            {
                UserId = finalUserId,
                PropertyTypeId = dto.PropertyTypeId,
                Title = dto.Title,
                Description = dto.Description,
                ShortDescription = dto.ShortDescription,
                ListingType = dto.ListingType,
                Status = "Active",
                Price = dto.Price,
                Currency = dto.Currency,
                IsNegotiable = dto.IsNegotiable,
                City = dto.City,
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                AreaSqm = dto.AreaSqm,
                FloorNumber = dto.FloorNumber,
                TotalFloors = dto.TotalFloors,
                YearBuilt = dto.YearBuilt,
                HasParking = dto.HasParking,
                HasBalcony = dto.HasBalcony,
                HasGarden = dto.HasGarden,
                HasElevator = dto.HasElevator,
                Street = dto.Street,
                PostCode = dto.PostCode,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                CreatedAt = DateTime.UtcNow,
                ViewsCount = 0
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = property.PropertyId }, new { id = property.PropertyId, title = property.Title });
        }

        [HttpPost("{propertyId}/media")]
        [Authorize]
        public async Task<IActionResult> AddMedia(int propertyId, [FromBody] List<string> imageUrls)
        {
            // 1. التأكد من وجود العقار
            var property = await _context.Properties.AnyAsync(p => p.PropertyId == propertyId);
            if (!property) return NotFound("العقار غير موجود");

            // 2. إضافة الروابط إلى جدول property_media
            foreach (var url in imageUrls)
            {
                var media = new PropertyMedia
                {
                    PropertyId = propertyId,
                    FileUrl = url,
                    IsPrimary = (imageUrls.IndexOf(url) == 0) // أول صورة في القائمة ستكون هي الأساسية
                };
                _context.PropertyMedia.Add(media);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم ربط الصور بالعقار بنجاح" });
        }

        // ==========================================
        // 4. PUT (تحديث)
        // ==========================================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyDto dto)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property == null)
                return NotFound();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || property.UserId != int.Parse(userIdClaim))
            {
                return Forbid("You are not authorized to update this property.");
            }

            property.PropertyTypeId = dto.PropertyTypeId;
            property.Title = dto.Title;
            property.Description = dto.Description;
            property.ShortDescription = dto.ShortDescription;
            property.ListingType = dto.ListingType;
            property.Status = dto.Status;
            property.Price = dto.Price;
            property.Currency = dto.Currency;
            property.IsNegotiable = dto.IsNegotiable;
            property.City = dto.City;
            property.Bedrooms = dto.Bedrooms;
            property.Bathrooms = dto.Bathrooms;
            property.AreaSqm = dto.AreaSqm;
            property.FloorNumber = dto.FloorNumber;
            property.TotalFloors = dto.TotalFloors;
            property.YearBuilt = dto.YearBuilt;
            property.HasParking = dto.HasParking;
            property.HasBalcony = dto.HasBalcony;
            property.HasGarden = dto.HasGarden;
            property.HasElevator = dto.HasElevator;
            property.Street = dto.Street;
            property.PostCode = dto.PostCode;
            property.Country = dto.Country;
            property.Latitude = dto.Latitude;
            property.Longitude = dto.Longitude;
            property.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Property updated successfully" });
        }

        // ==========================================
        // 5. DELETE (حذف)
        // ==========================================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property == null)
                return NotFound();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || property.UserId != int.Parse(userIdClaim))
            {
                return Forbid("You are not authorized to delete this property.");
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Property deleted successfully" });
        }


    }
}