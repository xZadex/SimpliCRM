using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpliCRM.Migrations
{
    public partial class FifthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Businesses_CompanyBusinessId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "CompanyBusinessId",
                table: "Employees",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_CompanyBusinessId",
                table: "Employees",
                newName: "IX_Employees_BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Businesses_BusinessId",
                table: "Employees",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Businesses_BusinessId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "Employees",
                newName: "CompanyBusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_BusinessId",
                table: "Employees",
                newName: "IX_Employees_CompanyBusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Businesses_CompanyBusinessId",
                table: "Employees",
                column: "CompanyBusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
