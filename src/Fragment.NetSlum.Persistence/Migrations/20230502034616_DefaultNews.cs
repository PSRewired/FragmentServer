using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DefaultNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_player_accounts_save_id",
                table: "player_accounts");

            migrationBuilder.CreateTable(
                name: "server_news",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_news", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "server_news",
                columns: new[] { "id", "content", "created_at" },
                values: new object[] { 1, "Welcome to Netslum-Redux!\nCurrent Status:\n- Lobby #GOnline#W!\n- BBS #GOnline#W!\n- Mail #GOnline#W!\n- Guilds #GOnline#W!\n- Ranking #GOnline#W!\n- News #GOnline#W!", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "ix_player_accounts_save_id",
                table: "player_accounts",
                column: "save_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "server_news");

            migrationBuilder.DropIndex(
                name: "ix_player_accounts_save_id",
                table: "player_accounts");

            migrationBuilder.CreateIndex(
                name: "ix_player_accounts_save_id",
                table: "player_accounts",
                column: "save_id");
        }
    }
}
