using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SahibindenMvc.Models;

[Index("ListingId", "AttributeDefId", Name = "UX_ListingAttrs_Listing_AttrDef", IsUnique = true)]
public partial class ListingAttribute
{
    [Key]
    public int ListingAttrId { get; set; }

    public int ListingId { get; set; }

    public int AttributeDefId { get; set; }

    [StringLength(200)]
    public string? ValueText { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ValueNumber { get; set; }

    [ForeignKey("AttributeDefId")]
    [InverseProperty("ListingAttributes")]
    public virtual AttributeDefinition AttributeDef { get; set; } = null!;

    [ForeignKey("ListingId")]
    [InverseProperty("ListingAttributes")]
    public virtual Listing Listing { get; set; } = null!;
}
