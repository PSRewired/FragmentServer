using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Core.Entities;

[Table("guilditemshop")]
public class Guilditemshop
{
    [Key]
    [Column("itemShopID")]
    public int ItemShopId { get; set; }

    [Column("guildID")]
    public int? GuildId { get; set; }

    [Column("itemID")]
    public int? ItemId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("generalPrice")]
    public int? GeneralPrice { get; set; }

    [Column("memberPrice")]
    public int? MemberPrice { get; set; }

    [Column("availableForGeneral")]
    public bool? AvailableForGeneral { get; set; }

    [Column("availableForMember")]
    public bool? AvailableForMember { get; set; }
}
