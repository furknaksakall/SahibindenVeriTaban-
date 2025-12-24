using System.ComponentModel.DataAnnotations;

namespace SahibindenMvc.Models
{
    public class ListingEditVm
    {
        [Required]
        public int ListingId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required, StringLength(160)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Range(1, 999999999)]
        public decimal Price { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "TRY";

        public bool IsActive { get; set; } = true;
    }
}
