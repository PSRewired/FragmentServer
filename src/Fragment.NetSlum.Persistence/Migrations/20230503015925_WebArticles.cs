using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WebArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "web_news_categories",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    category_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_news_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "web_news_articles",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    web_news_category_id = table.Column<int>(type: "int", nullable: true),
                    web_news_category_id1 = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_news_articles", x => x.id);
                    table.ForeignKey(
                        name: "fk_web_news_articles_web_news_categories_web_news_category_id1",
                        column: x => x.web_news_category_id1,
                        principalTable: "web_news_categories",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "web_news_read_logs",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_account_id = table.Column<int>(type: "int", nullable: false),
                    web_news_article_id = table.Column<int>(type: "int", nullable: false),
                    web_news_article_id1 = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_news_read_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_web_news_read_logs_player_accounts_player_account_id",
                        column: x => x.player_account_id,
                        principalTable: "player_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_web_news_read_logs_web_news_articles_web_news_article_id1",
                        column: x => x.web_news_article_id1,
                        principalTable: "web_news_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "web_news_categories",
                columns: new[] { "id", "category_name", "created_at" },
                values: new object[] { (ushort)1, "Netslum News", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "ix_web_news_articles_web_news_category_id1",
                table: "web_news_articles",
                column: "web_news_category_id1");

            migrationBuilder.CreateIndex(
                name: "ix_web_news_read_logs_player_account_id",
                table: "web_news_read_logs",
                column: "player_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_web_news_read_logs_web_news_article_id1",
                table: "web_news_read_logs",
                column: "web_news_article_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "web_news_read_logs");

            migrationBuilder.DropTable(
                name: "web_news_articles");

            migrationBuilder.DropTable(
                name: "web_news_categories");
        }
    }
}
