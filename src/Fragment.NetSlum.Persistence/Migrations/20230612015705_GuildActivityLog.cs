using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GuildActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "guild_membership_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    action_performed = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    guild_id = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    performed_by_character_id = table.Column<int>(type: "int", nullable: true),
                    performed_on_character_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    actioned_on = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guild_membership_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_guild_membership_logs_characters_performed_by_character_id",
                        column: x => x.performed_by_character_id,
                        principalTable: "characters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_guild_membership_logs_characters_performed_on_character_id",
                        column: x => x.performed_on_character_id,
                        principalTable: "characters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_guild_membership_logs_guilds_guild_id",
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
                name: "ix_guild_membership_logs_guild_id",
                table: "guild_membership_logs",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "ix_guild_membership_logs_performed_by_character_id",
                table: "guild_membership_logs",
                column: "performed_by_character_id");

            migrationBuilder.CreateIndex(
                name: "ix_guild_membership_logs_performed_on_character_id",
                table: "guild_membership_logs",
                column: "performed_on_character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guild_membership_logs");

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\r\nCurrent Status:\r\n- Lobby #GOnline#W!\r\n- BBS #GOnline#W!\r\n- Mail #GOnline#W!\r\n- Guilds #GOnline#W!\r\n- Ranking #GOnline#W!\r\n- News #GOnline#W!");
        }
    }
}
