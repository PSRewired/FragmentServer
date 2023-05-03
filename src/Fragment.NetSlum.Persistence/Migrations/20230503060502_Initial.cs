using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    save_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_accounts", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "server_news",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_news", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "web_news_categories",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    category_name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_news_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_account_id = table.Column<int>(type: "int", nullable: false),
                    character_name = table.Column<string>(type: "longtext", nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    greeting_message = table.Column<string>(type: "longtext", nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    save_slot_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    current_level = table.Column<int>(type: "int", nullable: false),
                    full_model_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    @class = table.Column<byte>(name: "class", type: "tinyint unsigned", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                    table.ForeignKey(
                        name: "fk_characters_player_accounts_player_account_id",
                        column: x => x.player_account_id,
                        principalTable: "player_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "web_news_articles",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    web_news_category_id = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    title = table.Column<string>(type: "varchar(33)", maxLength: 33, nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    content = table.Column<string>(type: "varchar(412)", maxLength: 412, nullable: false, collation: "sjis_japanese_ci")
                        .Annotation("MySql:CharSet", "sjis"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_news_articles", x => x.id);
                    table.ForeignKey(
                        name: "fk_web_news_articles_web_news_categories_web_news_category_id",
                        column: x => x.web_news_category_id,
                        principalTable: "web_news_categories",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "character_stat_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    character_id = table.Column<int>(type: "int", nullable: false),
                    current_hp = table.Column<int>(type: "int", nullable: false),
                    current_sp = table.Column<int>(type: "int", nullable: false),
                    current_gp = table.Column<uint>(type: "int unsigned", nullable: false),
                    online_treasures = table.Column<int>(type: "int", nullable: false),
                    average_field_level = table.Column<int>(type: "int", nullable: false),
                    gold_amount = table.Column<int>(type: "int", nullable: false),
                    silver_amount = table.Column<int>(type: "int", nullable: false),
                    bronze_amount = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_stat_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_stat_history_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "character_stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    character_id = table.Column<int>(type: "int", nullable: false),
                    current_hp = table.Column<int>(type: "int", nullable: false),
                    current_sp = table.Column<int>(type: "int", nullable: false),
                    current_gp = table.Column<uint>(type: "int unsigned", nullable: false),
                    online_treasures = table.Column<int>(type: "int", nullable: false),
                    average_field_level = table.Column<int>(type: "int", nullable: false),
                    gold_amount = table.Column<int>(type: "int", nullable: false),
                    silver_amount = table.Column<int>(type: "int", nullable: false),
                    bronze_amount = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_stats_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "web_news_read_logs",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_account_id = table.Column<int>(type: "int", nullable: false),
                    web_news_article_id = table.Column<ushort>(type: "smallint unsigned", nullable: false),
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
                        name: "fk_web_news_read_logs_web_news_articles_web_news_article_id",
                        column: x => x.web_news_article_id,
                        principalTable: "web_news_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "server_news",
                columns: new[] { "id", "content", "created_at" },
                values: new object[] { 1, "Welcome to Netslum-Redux!\nCurrent Status:\n- Lobby #GOnline#W!\n- BBS #GOnline#W!\n- Mail #GOnline#W!\n- Guilds #GOnline#W!\n- Ranking #GOnline#W!\n- News #GOnline#W!", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "web_news_categories",
                columns: new[] { "id", "category_name", "created_at" },
                values: new object[] { (ushort)1, "Netslum News", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "ix_character_stat_history_character_id",
                table: "character_stat_history",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_stat_history_created_at",
                table: "character_stat_history",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_character_stats_character_id",
                table: "character_stats",
                column: "character_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_stats_updated_at",
                table: "character_stats",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "ix_characters_player_account_id",
                table: "characters",
                column: "player_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_accounts_save_id",
                table: "player_accounts",
                column: "save_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_web_news_articles_web_news_category_id",
                table: "web_news_articles",
                column: "web_news_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_web_news_read_logs_player_account_id",
                table: "web_news_read_logs",
                column: "player_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_web_news_read_logs_web_news_article_id",
                table: "web_news_read_logs",
                column: "web_news_article_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_stat_history");

            migrationBuilder.DropTable(
                name: "character_stats");

            migrationBuilder.DropTable(
                name: "server_news");

            migrationBuilder.DropTable(
                name: "web_news_read_logs");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "web_news_articles");

            migrationBuilder.DropTable(
                name: "player_accounts");

            migrationBuilder.DropTable(
                name: "web_news_categories");
        }
    }
}
