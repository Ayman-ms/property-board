namespace Backend.Api.DTOs.Property
{
    public class PropertySearchDto
    {
        public string? City { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public string? ListingType { get; set; } // "Rent" or "Sale"

        // خصائص الترحيل (Pagination) ضرورية الآن
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}