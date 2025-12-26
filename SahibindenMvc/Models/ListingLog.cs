using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("ListingId", "LogDate", Name = "IX_ListingLogs_ListingId_LogDate", IsDescending = new[] { false, true })]
public partial class ListingLog
{
    [Key]
    public int LogId { get; set; }

    public int ListingId { get; set; }

    [StringLength(20)]
    public string ActionType { get; set; } = null!;

    [StringLength(160)]
    public string? OldTitle { get; set; }

    [StringLength(160)]
    public string? NewTitle { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OldPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? NewPrice { get; set; }

    public bool? OldIsActive { get; set; }

    public bool? NewIsActive { get; set; }

    public DateTime LogDate { get; set; }
}
