using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AreaServerAssociations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "area_server_associations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    auth_user_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    public_ip_address = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    local_ip_address = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_area_server_associations", x => x.id);
                    table.ForeignKey(
                        name: "fk_area_server_associations_auth_users_auth_user_id",
                        column: x => x.auth_user_id,
                        principalTable: "auth_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_area_server_associations_auth_user_id",
                table: "area_server_associations",
                column: "auth_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_area_server_associations_local_ip_address",
                table: "area_server_associations",
                column: "local_ip_address");

            migrationBuilder.CreateIndex(
                name: "ix_area_server_associations_public_ip_address",
                table: "area_server_associations",
                column: "public_ip_address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "area_server_associations");
        }
    }
}
