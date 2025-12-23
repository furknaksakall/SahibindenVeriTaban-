using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("CategoryId", Name = "IX_Listings_CategoryId")]
[Index("CreatedAt", Name = "IX_Listings_CreatedAt", AllDescending = true)]
[Index("LocationId", Name = "IX_Listings_LocationId")]
public partial class Listing
{
    [Key]
    public int ListingId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public int LocationId { get; set; }

    [StringLength(160)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Currency { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Listings")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Listing")]
    public virtual ICollection<ListingAttribute> ListingAttributes { get; set; } = new List<ListingAttribute>();

    [InverseProperty("Listing")]
    public virtual ICollection<ListingPhoto> ListingPhotos { get; set; } = new List<ListingPhoto>();

    [ForeignKey("LocationId")]
    [InverseProperty("Listings")]
    public virtual Location Location { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Listings")]
    public virtual User User { get; set; } = null!;
}
