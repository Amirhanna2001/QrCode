using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrCode.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserIdForQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QRPath",
                table: "QRCodes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "QRCodes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodes_UserId",
                table: "QRCodes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QRCodes_AspNetUsers_UserId",
                table: "QRCodes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCodes_AspNetUsers_UserId",
                table: "QRCodes");

            migrationBuilder.DropIndex(
                name: "IX_QRCodes_UserId",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QRCodes");

            migrationBuilder.AlterColumn<string>(
                name: "QRPath",
                table: "QRCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
