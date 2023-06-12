using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

public class GuildShopItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public ushort GuildId { get; set; }
    public Guild Guild { get; set; } = null!;

    public int ItemId { get; set; }
    public ushort Quantity { get; set; }
    public uint Price { get; set; }
    public uint MemberPrice { get; set; }
    public bool AvailableForMember { get; set; }
    public bool AvailableForGeneral { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Timestampable(EntityState.Modified)]
    public DateTime? UpdatedAt { get; set; }
}
