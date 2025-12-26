using System;

namespace SahibindenMvc.Models
{
    public class VwListingFilterResult
    {
        public int ListingId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Phone { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }

        public string? CategoryPath { get; set; }
        public string? LocationPath { get; set; }
        public string? DaysText { get; set; }
    }
}
