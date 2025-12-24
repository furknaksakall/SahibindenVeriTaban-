using System.ComponentModel.DataAnnotations;

namespace SahibindenMvc.Models
{
    public class CategoryCreateVm
    {
        public int? ParentId { get; set; }

        [Required, StringLength(120)]
        public string CategoryName { get; set; } = "";

        public int SortOrder { get; set; } = 0;
    }
}
