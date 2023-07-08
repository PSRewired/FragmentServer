using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Core.Constants;

namespace Fragment.NetSlum.Persistence.Entities;

public class ChatLobby : IConfigurableEntity<ChatLobby>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    [MaxLength(30)]
    public required string ChatLobbyName { get; set; }

    public ChatLobbyType LobbyType { get; set; } = ChatLobbyType.Default;

    public void Configure(EntityTypeBuilder<ChatLobby> entityBuilder)
    {
        // Store enum values as strings in the database
        entityBuilder
            .Property(e => e.LobbyType)
            .HasConversion(
                v => v.ToString(),
                v => (ChatLobbyType)Enum.Parse(typeof(ChatLobbyType), v));

        entityBuilder.HasData(new ChatLobby
        {
            Id = 1,
            ChatLobbyName = "Main",
            LobbyType = ChatLobbyType.Default,
        });
        entityBuilder.HasData(new ChatLobby
        {
            Id = 2,
            ChatLobbyName = "Main 2",
            LobbyType = ChatLobbyType.Default,
        });
    }
}
