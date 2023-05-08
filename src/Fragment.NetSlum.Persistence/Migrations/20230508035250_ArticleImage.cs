using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fragment.NetSlum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ArticleImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "web_news_articles",
                type: "longblob",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\nCurrent Status:\n- Lobby #GOnline#W!\n- BBS #GOnline#W!\n- Mail #GOnline#W!\n- Guilds #GOnline#W!\n- Ranking #GOnline#W!\n- News #GOnline#W!");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "web_news_articles");

            migrationBuilder.UpdateData(
                table: "server_news",
                keyColumn: "id",
                keyValue: 1,
                column: "content",
                value: "Welcome to Netslum-Redux!\r\nCurrent Status:\r\n- Lobby #GOnline#W!\r\n- BBS #GOnline#W!\r\n- Mail #GOnline#W!\r\n- Guilds #GOnline#W!\r\n- Ranking #GOnline#W!\r\n- News #GOnline#W!");
        }
    }
}
