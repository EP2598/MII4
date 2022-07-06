using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class add_category_column_to_ticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketCategory",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketCategory",
                table: "Tickets");
        }
    }
}
