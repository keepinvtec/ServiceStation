using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lr2ServiceStation.Migrations
{
    /// <inheritdoc />
    public partial class tpt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "YearsOfService",
                table: "Clients");

            migrationBuilder.CreateTable(
                name: "VIPs",
                columns: table => new
                {
                    PHnumber = table.Column<int>(type: "int", nullable: false),
                    YearsOfService = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIPs", x => x.PHnumber);
                    table.ForeignKey(
                        name: "FK_VIPs_Clients_PHnumber",
                        column: x => x.PHnumber,
                        principalTable: "Clients",
                        principalColumn: "PHnumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obsoletes",
                columns: table => new
                {
                    PHnumber = table.Column<int>(type: "int", nullable: false),
                    DateOfDeletion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obsoletes", x => x.PHnumber);
                    table.ForeignKey(
                        name: "FK_Obsoletes_VIPs_PHnumber",
                        column: x => x.PHnumber,
                        principalTable: "VIPs",
                        principalColumn: "PHnumber",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obsoletes");

            migrationBuilder.DropTable(
                name: "VIPs");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "PHnumber",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfService",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "PHnumber", "Discriminator", "FullName" },
                values: new object[] { 1, "Client", "Tom Cruise" });
        }
    }
}
