using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("Email", Name = "UQ__Users__A9D1053440A32DD4", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(120)]
    public string Email { get; set; } = null!;

    [StringLength(200)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
