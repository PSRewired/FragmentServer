using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class CharacterStats
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public int CurrentHp { get; set; }
    public int CurrentSp { get; set; }
    public uint CurrentGp { get; set; }
    public int GodOnline { get; set; }
    public int GodOffline { get; set; }
}
