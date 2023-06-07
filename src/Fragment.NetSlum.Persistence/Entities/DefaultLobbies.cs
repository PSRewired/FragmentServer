using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class DefaultLobbies : IConfigurableEntity<DefaultLobbies>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    [MaxLength(30)]
    public string DefaultLobbyName { get; set; }
   

    public void Configure(EntityTypeBuilder<DefaultLobbies> entityBuilder)
    {
        entityBuilder.HasData(new DefaultLobbies
        {
            Id = 1,
            DefaultLobbyName = "Main",
        });
        entityBuilder.HasData(new DefaultLobbies
        {
            Id = 2,
            DefaultLobbyName = "Main 2",
        });
    }
}
