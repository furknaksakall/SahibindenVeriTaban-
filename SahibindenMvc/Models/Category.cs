using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("ParentId", Name = "IX_Categories_ParentId")]
public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    public int? ParentId { get; set; }

    [StringLength(120)]
    public string CategoryName { get; set; } = null!;

    public int SortOrder { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<AttributeDefinition> AttributeDefinitions { get; set; } = new List<AttributeDefinition>();

    [InverseProperty("Parent")]
    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    [InverseProperty("Category")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Category? Parent { get; set; }
}
