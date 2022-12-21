using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lr2ServiceStation.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "5J8YE1H89NL032957",
                column: "Price",
                value: 114.0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WAUG8AFC1JN012500",
                column: "Price",
                value: 190.0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WP1AA2A2XGKA08083",
                column: "Price",
                value: 177.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "5J8YE1H89NL032957",
                column: "Price",
                value: 124.0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WAUG8AFC1JN012500",
                column: "Price",
                value: 126.0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WP1AA2A2XGKA08083",
                column: "Price",
                value: 127.0);
        }
    }
}
