using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatLobbiesForPlayerAndGuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "guild_lobby",
                table: "chat_lobbies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "player_lobby",
                table: "chat_lobbies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "chat_lobbies",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "guild_lobby", "player_lobby" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "chat_lobbies",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "guild_lobby", "player_lobby" },
                values: new object[] { false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guild_lobby",
                table: "chat_lobbies");

            migrationBuilder.DropColumn(
                name: "player_lobby",
                table: "chat_lobbies");
        }
    }
}
