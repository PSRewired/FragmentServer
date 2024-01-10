using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CharSaveSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_guilds_characters_leader_id1",
                table: "guilds");

            migrationBuilder.AddColumn<string>(
                name: "save_id",
                table: "characters",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "fk_guilds_characters_leader_id",
                table: "guilds",
                column: "leader_id",
                principalTable: "characters",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_guilds_characters_leader_id",
                table: "guilds");

            migrationBuilder.DropColumn(
                name: "save_id",
                table: "characters");

            migrationBuilder.AddForeignKey(
                name: "fk_guilds_characters_leader_id1",
                table: "guilds",
                column: "leader_id",
                principalTable: "characters",
                principalColumn: "id");
        }
    }
}
