using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class DefaultLobby : IConfigurableEntity<DefaultLobby>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    [MaxLength(30)]
    public required string DefaultLobbyName { get; set; }


    public void Configure(EntityTypeBuilder<DefaultLobby> entityBuilder)
    {
        entityBuilder.HasData(new DefaultLobby
        {
            Id = 1,
            DefaultLobbyName = "Main",
        });
        entityBuilder.HasData(new DefaultLobby
        {
            Id = 2,
            DefaultLobbyName = "Main 2",
        });
    }
}
