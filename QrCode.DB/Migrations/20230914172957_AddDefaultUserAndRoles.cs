using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QrCode.DB.Migrations
{
    public partial class AddDefaultUserAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert roles
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, [Name], NormalizedName) VALUES ('dde410a9-d23c-42a4-b407-1e92eb955021', 'Admin', 'ADMIN')");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, [Name], NormalizedName) VALUES ('e01d03bc-4b39-4ded-a2aa-c591324f00ac', 'Users', 'USERS')");

            // Insert the default user
            migrationBuilder.Sql("INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, AccessFailedCount,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled) " +
            "VALUES ('aea2f5bb-d259-49e3-b890-8c4b0d7715c0', 'SuperAdmin', 'SUPERADMIN', 'SuperAdmin@QrCodeGenerator.com', 'SUPERADMIN@QRCODEGENERATOR.COM', 1, 'AQAAAAEAACcQAAAAEJf4Pr6S78QtKEL7LB/u9/woXVU4Sq9jtuqyDlRyr2BtKVuoD1jeCeD2v1G+tf57kg==', 'b71cd6d7-ad63-43fa-ba41-c022234c4715', 0,0,0,0)");

            // Assign roles to the default user
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('aea2f5bb-d259-49e3-b890-8c4b0d7715c0', 'dde410a9-d23c-42a4-b407-1e92eb955021')");
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('aea2f5bb-d259-49e3-b890-8c4b0d7715c0', 'e01d03bc-4b39-4ded-a2aa-c591324f00ac')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the data seeding for roles and the default user
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId = 'aea2f5bb-d259-49e3-b890-8c4b0d7715c0'");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id IN ('dde410a9-d23c-42a4-b407-1e92eb955021', 'e01d03bc-4b39-4ded-a2aa-c591324f00ac')");
            migrationBuilder.Sql("DELETE FROM AspNetUsers WHERE Id = 'aea2f5bb-d259-49e3-b890-8c4b0d7715c0'");
        }
    }
}
