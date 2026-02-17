using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.Other
{
    public class SavedSearchDto : BaseDto
    {
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string SearchName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public int? PropertyTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public bool? IsFurnished { get; set; }
        public bool? HasGarden { get; set; }
        public bool? HasPool { get; set; }
        public bool? HasElevator { get; set; }
        public bool? HasBalcony { get; set; }
        public bool? HasAc { get; set; }
        public bool IsActive { get; set; }
        public bool EmailNotifications { get; set; }
        
        // Property Type Name
        public string? PropertyTypeName { get; set; }
        
        // Helper Properties
        public string PriceRange => GetPriceRange();
        public string BedroomRange => GetBedroomRange();
        public string BathroomRange => GetBathroomRange();
        public string AreaRange => GetAreaRange();
        public List<string> Features => GetSearchFeatures();
        
        private string GetPriceRange()
        {
            if (MinPrice.HasValue && MaxPrice.HasValue)
                return $"{MinPrice:N0} - {MaxPrice:N0}";
            if (MinPrice.HasValue)
                return $"From {MinPrice:N0}";
            if (MaxPrice.HasValue)
                return $"Up to {MaxPrice:N0}";
            return "Any price";
        }
        
        private string GetBedroomRange()
        {
            if (MinBedrooms.HasValue && MaxBedrooms.HasValue && MinBedrooms == MaxBedrooms)
                return $"{MinBedrooms} bedrooms";
            if (MinBedrooms.HasValue && MaxBedrooms.HasValue)
                return $"{MinBedrooms}-{MaxBedrooms} bedrooms";
            if (MinBedrooms.HasValue)
                return $"{MinBedrooms}+ bedrooms";
            if (MaxBedrooms.HasValue)
                return $"Up to {MaxBedrooms} bedrooms";
            return "Any bedrooms";
        }
        
        private string GetBathroomRange()
        {
            if (MinBathrooms.HasValue && MaxBathrooms.HasValue && MinBathrooms == MaxBathrooms)
                return $"{MinBathrooms} bathrooms";
            if (MinBathrooms.HasValue && MaxBathrooms.HasValue)
                return $"{MinBathrooms}-{MaxBathrooms} bathrooms";
            if (MinBathrooms.HasValue)
                return $"{MinBathrooms}+ bathrooms";
            if (MaxBathrooms.HasValue)
                return $"Up to {MaxBathrooms} bathrooms";
            return "Any bathrooms";
        }
        
        private string GetAreaRange()
        {
            if (MinArea.HasValue && MaxArea.HasValue)
                return $"{MinArea:N0} - {MaxArea:N0} sqm";
            if (MinArea.HasValue)
                return $"From {MinArea:N0} sqm";
            if (MaxArea.HasValue)
                return $"Up to {MaxArea:N0} sqm";
            return "Any area";
        }
        
        private List<string> GetSearchFeatures()
        {
            var features = new List<string>();
            if (IsFurnished == true) features.Add("Furnished");
            if (HasGarden == true) features.Add("Garden");
            if (HasPool == true) features.Add("Pool");
            if (HasElevator == true) features.Add("Elevator");
            if (HasBalcony == true) features.Add("Balcony");
            if (HasAc == true) features.Add("Air Conditioning");
            return features;
        }
    }
}