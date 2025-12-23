using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("ParentId", Name = "IX_Locations_ParentId")]
public partial class Location
{
    [Key]
    public int LocationId { get; set; }

    public int? ParentId { get; set; }

    [StringLength(100)]
    public string LocationName { get; set; } = null!;

    public byte LocationType { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Location> InverseParent { get; set; } = new List<Location>();

    [InverseProperty("Location")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Location? Parent { get; set; }
}
