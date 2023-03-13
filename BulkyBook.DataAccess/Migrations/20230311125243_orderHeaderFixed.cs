using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBookWeb.Migrations
{
    /// <inheritdoc />
    public partial class orderHeaderFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TarckingNumber",
                table: "OrderHeaders",
                newName: "TrackingNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackingNumber",
                table: "OrderHeaders",
                newName: "TarckingNumber");
        }
    }
}
