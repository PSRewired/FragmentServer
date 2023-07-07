using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DefaultPlayerLobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "chat_lobbies",
                columns: new[] { "id", "chat_lobby_name", "lobby_type" },
                values: new object[] { 3, "General", "Player" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "chat_lobbies",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
