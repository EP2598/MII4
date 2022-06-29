using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class update_account_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_Id",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Employees_Id",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_Id",
                table: "Accounts",
                column: "Id",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Employees_Id",
                table: "Accounts",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
