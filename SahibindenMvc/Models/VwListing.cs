using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Keyless]
public partial class VwListing
{
    public int ListingId { get; set; }

    public int UserId { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    public int CategoryId { get; set; }

    [StringLength(120)]
    public string CategoryName { get; set; } = null!;

    public int LocationId { get; set; }

    public int? CityId { get; set; }

    [StringLength(100)]
    public string? CityName { get; set; }

    public int? DistrictId { get; set; }

    [StringLength(100)]
    public string? DistrictName { get; set; }

    [StringLength(160)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Currency { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    [StringLength(400)]
    public string? CategoryPath { get; set; }

    [StringLength(300)]
    public string? LocationPath { get; set; }

    [StringLength(50)]
    public string? DaysText { get; set; }
}
