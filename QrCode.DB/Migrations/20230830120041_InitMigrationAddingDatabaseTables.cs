using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrCode.DB.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrationAddingDatabaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QRCodes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashValiue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QRPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRCodes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "QRScans",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QRCodeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRScans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QRScans_QRCodes_QRCodeID",
                        column: x => x.QRCodeID,
                        principalTable: "QRCodes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_QRCodeID",
                table: "QRScans",
                column: "QRCodeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QRScans");

            migrationBuilder.DropTable(
                name: "QRCodes");
        }
    }
}
