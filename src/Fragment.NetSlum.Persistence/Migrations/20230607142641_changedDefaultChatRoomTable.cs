﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changedDefaultChatRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\r\nCurrent Status:\r\n- Lobby #GOnline#W!\r\n- BBS #GOnline#W!\r\n- Mail #GOnline#W!\r\n- Guilds #GOnline#W!\r\n- Ranking #GOnline#W!\r\n- News #GOnline#W!");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "default_lobbies");

            migrationBuilder.CreateTable(
                name: "chat_lobbies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    chat_lobby_name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    default_channel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    guild_lobby = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    player_lobby = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_lobbies", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "chat_lobbies",
                columns: new[] { "id", "chat_lobby_name", "default_channel", "guild_lobby", "player_lobby" },
                values: new object[,]
                {
                    { 1, "Main", true, false, false },
                    { 2, "Main 2", true, false, false }
                });

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\nCurrent Status:\n- Lobby #GOnline#W!\n- BBS #GOnline#W!\n- Mail #GOnline#W!\n- Guilds #GOnline#W!\n- Ranking #GOnline#W!\n- News #GOnline#W!");
        }
    }
}
