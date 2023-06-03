using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixEncoding2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "category_name",
                table: "web_news_categories",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64,
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "web_news_articles",
                type: "varchar(33)",
                maxLength: 33,
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(33)",
                oldMaxLength: 33,
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "web_news_articles",
                type: "varchar(412)",
                maxLength: 412,
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(412)",
                oldMaxLength: 412,
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "server_news",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "guilds",
                type: "longtext",
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "guilds",
                type: "longtext",
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "chat_lobby_name",
                table: "chat_lobbies",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "greeting_message",
                table: "characters",
                type: "longtext",
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");

            migrationBuilder.AlterColumn<string>(
                name: "character_name",
                table: "characters",
                type: "longtext",
                nullable: false,
                collation: "cp932_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "ujis_japanese_ci")
                .Annotation("MySql:CharSet", "cp932")
                .OldAnnotation("MySql:CharSet", "ujis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "category_name",
                table: "web_news_categories",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64,
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "web_news_articles",
                type: "varchar(33)",
                maxLength: 33,
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(33)",
                oldMaxLength: 33,
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "web_news_articles",
                type: "varchar(412)",
                maxLength: 412,
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(412)",
                oldMaxLength: 412,
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "server_news",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "guilds",
                type: "longtext",
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "guilds",
                type: "longtext",
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "chat_lobby_name",
                table: "chat_lobbies",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "greeting_message",
                table: "characters",
                type: "longtext",
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");

            migrationBuilder.AlterColumn<string>(
                name: "character_name",
                table: "characters",
                type: "longtext",
                nullable: false,
                collation: "ujis_japanese_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldCollation: "cp932_japanese_ci")
                .Annotation("MySql:CharSet", "ujis")
                .OldAnnotation("MySql:CharSet", "cp932");
        }
    }
}
