using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class CharacterCurrency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public int GoldAmount { get; set; }
    public int SilverAmount { get; set; }
    public int BronzeAmount { get; set; }
}
