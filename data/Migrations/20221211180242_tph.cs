using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lr2ServiceStation.Migrations
{
    /// <inheritdoc />
    public partial class tph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "YearsOfService",
                table: "Clients");
        }
    }
}
