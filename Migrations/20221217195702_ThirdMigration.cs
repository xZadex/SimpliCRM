using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpliCRM.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Owners_BusinessOwnerOwnerId",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "BusinessOwnerOwnerId",
                table: "Businesses",
                newName: "BusinessOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Businesses_BusinessOwnerOwnerId",
                table: "Businesses",
                newName: "IX_Businesses_BusinessOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Owners_BusinessOwnerId",
                table: "Businesses",
                column: "BusinessOwnerId",
                principalTable: "Owners",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Owners_BusinessOwnerId",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "BusinessOwnerId",
                table: "Businesses",
                newName: "BusinessOwnerOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Businesses_BusinessOwnerId",
                table: "Businesses",
                newName: "IX_Businesses_BusinessOwnerOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Owners_BusinessOwnerOwnerId",
                table: "Businesses",
                column: "BusinessOwnerOwnerId",
                principalTable: "Owners",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
