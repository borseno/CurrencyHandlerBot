using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyHandler.Migrations
{
    public partial class ChangedArrayToList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatSettings",
                columns: table => new
                {
                    ChatId = table.Column<long>(nullable: false),
                    Percents = table.Column<decimal>(nullable: false, defaultValue: 100m),
                    ValueCurrency = table.Column<string>(nullable: false, defaultValue: "UAH"),
                    DisplayCurrencies = table.Column<string>(nullable: false, defaultValue: "UAH,RUB,EUR,USD,BYN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSettings", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyEmojis",
                columns: table => new
                {
                    Currency = table.Column<string>(nullable: false),
                    Emoji = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyEmojis", x => x.Currency);
                });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "AUD", "🇦🇺" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "GBP", "🇬🇧" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "DKK", "🇩🇰" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "USD", "🇺🇸" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "EUR", "🇪🇺" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "BYN", "🇧🇾" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "KZT", "🇰🇿" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "CAD", "🇨🇦" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "NOK", "🇳🇴" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "SGD", "🇸🇬" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "TRY", "🇹🇷" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "UAH", "🇺🇦" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "SEK", "🇸🇪" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "CHF", "🇨🇭" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "JPY", "🇯🇵" });

            migrationBuilder.InsertData(
                table: "CurrencyEmojis",
                columns: new[] { "Currency", "Emoji" },
                values: new object[] { "RUB", "🇷🇺" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatSettings");

            migrationBuilder.DropTable(
                name: "CurrencyEmojis");
        }
    }
}
