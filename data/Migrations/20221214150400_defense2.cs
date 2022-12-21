using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lr2ServiceStation.Migrations
{
    /// <inheritdoc />
    public partial class defense2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Cars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "VINcode", "Brand", "EngDisplacement", "Manufacture", "Mileage", "Model", "Price", "YearOfProd" },
                values: new object[,]
                {
                    { "19UUB7F02MA000899", "Acura", 2986.0, "Honda", 12368, "TLX", 123.0, 2021 },
                    { "1GYFK43519R118886", "Cadillac", 6162.0, "GM", 24571, "Escalade", 129.0, 2007 },
                    { "2T2HZMAAXNC228796", "Lexus", 2394.0, "Toyota", 2315, "RX", 125.0, 2021 },
                    { "5J8YE1H89NL032957", "Acura", 2986.0, "Honda", 5429, "MDX", 124.0, 2021 },
                    { "SALGS2TF9FA225873", "Land Rover", 4999.0, "JLR", 111567, "Range Rover", 130.0, 2013 },
                    { "WAUG8AFC1JN012500", "Audi", 2984.0, "VAG", 42156, "A6", 126.0, 2018 },
                    { "WP1AA2A2XGKA08083", "Skoda", 1990.0, "VAG", 100344, "Octavia", 127.0, 2016 },
                    { "WVGFF9BP8BD000455", "Volkswagen", 2986.0, "VAG", 156302, "Touareg", 128.0, 2014 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "19UUB7F02MA000899");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "1GYFK43519R118886");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "2T2HZMAAXNC228796");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "5J8YE1H89NL032957");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "SALGS2TF9FA225873");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WAUG8AFC1JN012500");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WP1AA2A2XGKA08083");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "VINcode",
                keyValue: "WVGFF9BP8BD000455");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cars");
        }
    }
}
