using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("CategoryId", Name = "IX_AttrDefs_CategoryId")]
public partial class AttributeDefinition
{
    [Key]
    public int AttributeDefId { get; set; }

    public int CategoryId { get; set; }

    [StringLength(80)]
    public string AttributeName { get; set; } = null!;

    public byte DataType { get; set; }

    public bool IsRequired { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("AttributeDefinitions")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("AttributeDef")]
    public virtual ICollection<ListingAttribute> ListingAttributes { get; set; } = new List<ListingAttribute>();
}
