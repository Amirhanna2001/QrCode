using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrCode.DB.Migrations
{
    /// <inheritdoc />
    public partial class addingIsDeletedColums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QRScans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QRCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QRScans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QRCodes");
        }
    }
}
