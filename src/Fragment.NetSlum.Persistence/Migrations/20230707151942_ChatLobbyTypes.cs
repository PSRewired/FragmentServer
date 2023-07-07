using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatLobbyTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "default_lobbies");

            migrationBuilder.CreateTable(
                name: "chat_lobbies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    chat_lobby_name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    lobby_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_lobbies", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "chat_lobbies",
                columns: new[] { "id", "chat_lobby_name", "lobby_type" },
                values: new object[,]
                {
                    { 1, "Main", "Default" },
                    { 2, "Main 2", "Default" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_lobbies");

            migrationBuilder.CreateTable(
                name: "default_lobbies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    default_lobby_name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_default_lobbies", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "default_lobbies",
                columns: new[] { "id", "default_lobby_name" },
                values: new object[,]
                {
                    { 1, "Main" },
                    { 2, "Main 2" }
                });
        }
    }
}
