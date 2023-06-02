using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Guilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ushort>(
                name: "guild_id",
                table: "characters",
                type: "smallint unsigned",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "guilds",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    comment = table.Column<string>(type: "longtext", nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    emblem = table.Column<byte[]>(type: "longblob", nullable: false),
                    leader_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guilds", x => x.id);
                    table.ForeignKey(
                        name: "fk_guilds_characters_leader_id",
                        column: x => x.leader_id,
                        principalTable: "characters",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "guild_stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guild_id = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    gold_amount = table.Column<int>(type: "int", nullable: false),
                    silver_amount = table.Column<int>(type: "int", nullable: false),
                    bronze_amount = table.Column<int>(type: "int", nullable: false),
                    current_gp = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guild_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_guild_stats_guilds_guild_id",
                        column: x => x.guild_id,
                        principalTable: "guilds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\nCurrent Status:\n- Lobby #GOnline#W!\n- BBS #GOnline#W!\n- Mail #GOnline#W!\n- Guilds #GOnline#W!\n- Ranking #GOnline#W!\n- News #GOnline#W!");

            migrationBuilder.CreateIndex(
                name: "ix_characters_guild_id",
                table: "characters",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "ix_guild_stats_guild_id",
                table: "guild_stats",
                column: "guild_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_guilds_leader_id",
                table: "guilds",
                column: "leader_id");

            migrationBuilder.AddForeignKey(
                name: "fk_characters_guilds_guild_id",
                table: "characters",
                column: "guild_id",
                principalTable: "guilds",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_characters_guilds_guild_id",
                table: "characters");

            migrationBuilder.DropTable(
                name: "guild_stats");

            migrationBuilder.DropTable(
                name: "guilds");

            migrationBuilder.DropIndex(
                name: "ix_characters_guild_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "guild_id",
                table: "characters");

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\r\nCurrent Status:\r\n- Lobby #GOnline#W!\r\n- BBS #GOnline#W!\r\n- Mail #GOnline#W!\r\n- Guilds #GOnline#W!\r\n- Ranking #GOnline#W!\r\n- News #GOnline#W!");
        }
    }
}
