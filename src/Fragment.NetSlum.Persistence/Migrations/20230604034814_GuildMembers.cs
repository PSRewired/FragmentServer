using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GuildMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_guilds_characters_leader_id",
                table: "guilds");

            migrationBuilder.DropIndex(
                name: "ix_guilds_leader_id",
                table: "guilds");

            migrationBuilder.CreateIndex(
                name: "ix_guilds_leader_id",
                table: "guilds",
                column: "leader_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_guilds_characters_leader_id1",
                table: "guilds",
                column: "leader_id",
                principalTable: "characters",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_guilds_characters_leader_id1",
                table: "guilds");

            migrationBuilder.DropIndex(
                name: "ix_guilds_leader_id",
                table: "guilds");

            migrationBuilder.CreateIndex(
                name: "ix_guilds_leader_id",
                table: "guilds",
                column: "leader_id");

            migrationBuilder.AddForeignKey(
                name: "fk_guilds_characters_leader_id",
                table: "guilds",
                column: "leader_id",
                principalTable: "characters",
                principalColumn: "id");
        }
    }
}
