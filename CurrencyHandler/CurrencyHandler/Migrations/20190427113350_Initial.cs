using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyHandler.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatSettings",
                columns: table => new
                {
                    ChatSettingsId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChatId = table.Column<long>(nullable: false),
                    Percents = table.Column<decimal>(nullable: false, defaultValue: 100m),
                    Currency = table.Column<string>(nullable: true, defaultValue: "UAH")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSettings", x => x.ChatSettingsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatSettings");
        }
    }
}
