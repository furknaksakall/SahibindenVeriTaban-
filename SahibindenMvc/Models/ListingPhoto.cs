using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("ListingId", Name = "IX_ListingPhotos_ListingId")]
public partial class ListingPhoto
{
    [Key]
    public int PhotoId { get; set; }

    public int ListingId { get; set; }

    [StringLength(300)]
    public string PhotoUrl { get; set; } = null!;

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("ListingId")]
    [InverseProperty("ListingPhotos")]
    public virtual Listing Listing { get; set; } = null!;
}
