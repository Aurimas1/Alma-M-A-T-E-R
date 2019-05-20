using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddIsApartmentNeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("IsApartmentNeeded", "EmployeeToTrips", nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
