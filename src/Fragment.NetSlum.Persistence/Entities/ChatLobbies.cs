using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities
{
    public class ChatLobbies : IConfigurableEntity<ChatLobbies>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MySqlCharSet("sjis")]
        [MySqlCollation("sjis_japanese_ci")]
        [MaxLength(30)]
        public string ChatLobbyName { get; set; }
        public bool DefaultChannel { get; set; } = false;
        public bool PlayerLobby { get; set; } = false;
        public bool GuildLobby { get; set; } = false;

        public void Configure(EntityTypeBuilder<ChatLobbies> entityBuilder)
        {
            entityBuilder.HasData(new ChatLobbies
            {
                Id = 1,
                ChatLobbyName = "Main",
                DefaultChannel = true,
            });
            entityBuilder.HasData(new ChatLobbies
            {
                Id = 2,
                ChatLobbyName = "Main 2",
                DefaultChannel = true,
            });
        }
    }
}
