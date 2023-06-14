using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                name: "area_server_categories",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    category_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_area_server_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bbs_categories",
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
                    table.PrimaryKey("pk_bbs_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "player_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    save_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
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
                    content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
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
                    category_name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_news_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bbs_threads",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    category_id = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bbs_threads", x => x.id);
                    table.ForeignKey(
                        name: "fk_bbs_threads_bbs_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "bbs_categories",
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
                    title = table.Column<string>(type: "varchar(33)", maxLength: 33, nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    content = table.Column<string>(type: "varchar(412)", maxLength: 412, nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    image = table.Column<byte[]>(type: "longblob", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "bbs_post_contents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bbs_post_contents", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bbs_posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    thread_id = table.Column<int>(type: "int", nullable: false),
                    posted_by_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bbs_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_bbs_posts_bbs_threads_thread_id",
                        column: x => x.thread_id,
                        principalTable: "bbs_threads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_account_id = table.Column<int>(type: "int", nullable: false),
                    character_name = table.Column<string>(type: "longtext", nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    greeting_message = table.Column<string>(type: "longtext", nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    save_slot_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    current_level = table.Column<int>(type: "int", nullable: false),
                    full_model_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    @class = table.Column<byte>(name: "class", type: "tinyint unsigned", nullable: false),
                    guild_id = table.Column<ushort>(type: "smallint unsigned", nullable: true),
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
                name: "guilds",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    comment = table.Column<string>(type: "longtext", nullable: false, collation: "cp932_japanese_ci")
                        .Annotation("MySql:CharSet", "cp932"),
                    emblem = table.Column<byte[]>(type: "longblob", nullable: false),
                    leader_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guilds", x => x.id);
                    table.ForeignKey(
                        name: "fk_guilds_characters_leader_id1",
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

            migrationBuilder.InsertData(
                table: "area_server_categories",
                columns: new[] { "id", "category_name" },
                values: new object[,]
                {
                    { (ushort)1, "Main" },
                    { (ushort)2, "Test" }
                });

            migrationBuilder.InsertData(
                table: "bbs_categories",
                columns: new[] { "id", "category_name", "created_at" },
                values: new object[] { (ushort)1, "GENERAL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "default_lobbies",
                columns: new[] { "id", "default_lobby_name" },
                values: new object[,]
                {
                    { 1, "Main" },
                    { 2, "Main 2" }
                });

            migrationBuilder.InsertData(
                table: "server_news",
                columns: new[] { "id", "content", "created_at" },
                values: new object[] { 1, "Welcome to Netslum-Redux!\nCurrent Status:\n- Lobby #GOnline#W!\n- BBS #GOnline#W!\n- Mail #GOnline#W!\n- Guilds #GOnline#W!\n- Ranking #GOnline#W!\n- News #GOnline#W!", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "web_news_categories",
                columns: new[] { "id", "category_name", "created_at" },
                values: new object[] { (ushort)1, "Netslum News", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "ix_bbs_post_contents_post_id",
                table: "bbs_post_contents",
                column: "post_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bbs_posts_posted_by_id",
                table: "bbs_posts",
                column: "posted_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_bbs_posts_thread_id",
                table: "bbs_posts",
                column: "thread_id");

            migrationBuilder.CreateIndex(
                name: "ix_bbs_threads_category_id",
                table: "bbs_threads",
                column: "category_id");

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
                name: "ix_characters_guild_id",
                table: "characters",
                column: "guild_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_player_account_id",
                table: "characters",
                column: "player_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_guild_stats_guild_id",
                table: "guild_stats",
                column: "guild_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_guilds_leader_id",
                table: "guilds",
                column: "leader_id",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "fk_bbs_post_contents_bbs_posts_post_id",
                table: "bbs_post_contents",
                column: "post_id",
                principalTable: "bbs_posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bbs_posts_characters_posted_by_id",
                table: "bbs_posts",
                column: "posted_by_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_history_characters_character_id",
                table: "character_stat_history",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_character_stats_characters_character_id",
                table: "character_stats",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
                name: "fk_guilds_characters_leader_id1",
                table: "guilds");

            migrationBuilder.DropTable(
                name: "area_server_categories");

            migrationBuilder.DropTable(
                name: "bbs_post_contents");

            migrationBuilder.DropTable(
                name: "character_stat_history");

            migrationBuilder.DropTable(
                name: "character_stats");

            migrationBuilder.DropTable(
                name: "default_lobbies");

            migrationBuilder.DropTable(
                name: "guild_stats");

            migrationBuilder.DropTable(
                name: "server_news");

            migrationBuilder.DropTable(
                name: "web_news_read_logs");

            migrationBuilder.DropTable(
                name: "bbs_posts");

            migrationBuilder.DropTable(
                name: "web_news_articles");

            migrationBuilder.DropTable(
                name: "bbs_threads");

            migrationBuilder.DropTable(
                name: "web_news_categories");

            migrationBuilder.DropTable(
                name: "bbs_categories");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "guilds");

            migrationBuilder.DropTable(
                name: "player_accounts");
        }
    }
}
