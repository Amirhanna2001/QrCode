using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrCode.DB.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnHashValiueToHashValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashValiue",
                table: "QRCodes",
                newName: "HashValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashValue",
                table: "QRCodes",
                newName: "HashValiue");
        }
    }
}
