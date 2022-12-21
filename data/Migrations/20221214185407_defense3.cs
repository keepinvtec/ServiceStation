using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lr2ServiceStation.Migrations
{
    /// <inheritdoc />
    public partial class defense3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfSale",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "19UUB7F02MA000899",
                column: "YearOfSale",
                value: 2022);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "1GYFK43519R118886",
                column: "YearOfSale",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "2T2HZMAAXNC228796",
                column: "YearOfSale",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "5J8YE1H89NL032957",
                column: "YearOfSale",
                value: 2022);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "SALGS2TF9FA225873",
                column: "YearOfSale",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WAUG8AFC1JN012500",
                column: "YearOfSale",
                value: 2022);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WP1AA2A2XGKA08083",
                column: "YearOfSale",
                value: 2021);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WVGFF9BP8BD000455",
                column: "YearOfSale",
                value: 2020);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearOfSale",
                table: "Cars");
        }
    }
}
