using Backend.Api.Models;
using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Property;

namespace Backend.Api.Repository.Interfaces
{
    public interface IPropertyRepository
    {
        Task<PaginatedResultDto<Property>> GetPropertiesPaginatedAsync(PaginationDto pagination);

        Task<PaginatedResultDto<Property>> SearchPropertiesAsync(PropertySearchDto searchDto);

        Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int userId);

        Task<IEnumerable<Property>> GetFeaturedPropertiesAsync(int count);

        Task<Property?> GetPropertyWithDetailsAsync(int propertyId);

        Task<bool> UpdateViewCountAsync(int propertyId);

        Task<decimal> GetAverageRatingAsync(int propertyId);

        Task<IEnumerable<Property>> GetAllAsync();
        Task<Property?> GetByIdAsync(int id);
        Task<Property> AddAsync(Property property);
        Task AddMediaAsync(PropertyMedia media);
        Task<IEnumerable<PropertyMedia>> GetMediaByPropertyIdAsync(int propertyId);

    }
}
