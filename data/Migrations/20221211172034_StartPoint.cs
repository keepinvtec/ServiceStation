using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lr2ServiceStation.Migrations
{
    /// <inheritdoc />
    public partial class StartPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    VINcode = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Manufacture = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EngDisplacement = table.Column<double>(type: "float", nullable: false),
                    YearOfProd = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.VINcode);
                    table.UniqueConstraint("AK_Cars_VINcode_EngDisplacement_Mileage", x => new { x.VINcode, x.EngDisplacement, x.Mileage });
                    table.CheckConstraint("YearOfProd", "YearOfProd > 2000 AND YearOfProd < 2022");
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    PHnumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.PHnumber);
                });

            migrationBuilder.CreateTable(
                name: "ListsOfServices",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOfService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complexity = table.Column<double>(type: "float", nullable: false),
                    CostOfAnHour = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsOfServices", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfEnroll = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ClientPHnumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Clients_ClientPHnumber",
                        column: x => x.ClientPHnumber,
                        principalTable: "Clients",
                        principalColumn: "PHnumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientPHnumber = table.Column<int>(type: "int", nullable: false),
                    CarVINcode = table.Column<string>(type: "nvarchar(18)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Cars_CarVINcode",
                        column: x => x.CarVINcode,
                        principalTable: "Cars",
                        principalColumn: "VINcode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientPHnumber",
                        column: x => x.ClientPHnumber,
                        principalTable: "Clients",
                        principalColumn: "PHnumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListsOfProvidedServices",
                columns: table => new
                {
                    ListOfProvidedServicesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceOrderId = table.Column<int>(type: "int", nullable: false),
                    ListOfServicesServiceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsOfProvidedServices", x => x.ListOfProvidedServicesId);
                    table.ForeignKey(
                        name: "FK_ListsOfProvidedServices_Invoices_InvoiceOrderId",
                        column: x => x.InvoiceOrderId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListsOfProvidedServices_ListsOfServices_ListOfServicesServiceID",
                        column: x => x.ListOfServicesServiceID,
                        principalTable: "ListsOfServices",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListsOfSpareParts",
                columns: table => new
                {
                    SPnumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOfPart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    InvoiceOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsOfSpareParts", x => x.SPnumber);
                    table.ForeignKey(
                        name: "FK_ListsOfSpareParts_Invoices_InvoiceOrderId",
                        column: x => x.InvoiceOrderId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "PHnumber", "FullName" },
                values: new object[] { 1, "Tom Cruise" });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentId", "ClientPHnumber" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ClientPHnumber",
                table: "Enrollments",
                column: "ClientPHnumber");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CarVINcode",
                table: "Invoices",
                column: "CarVINcode");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientPHnumber",
                table: "Invoices",
                column: "ClientPHnumber");

            migrationBuilder.CreateIndex(
                name: "IX_ListsOfProvidedServices_InvoiceOrderId",
                table: "ListsOfProvidedServices",
                column: "InvoiceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ListsOfProvidedServices_ListOfServicesServiceID",
                table: "ListsOfProvidedServices",
                column: "ListOfServicesServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ListsOfSpareParts_InvoiceOrderId",
                table: "ListsOfSpareParts",
                column: "InvoiceOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "ListsOfProvidedServices");

            migrationBuilder.DropTable(
                name: "ListsOfSpareParts");

            migrationBuilder.DropTable(
                name: "ListsOfServices");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
