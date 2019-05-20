using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddIsApartmentNeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("IsApartmentNeeded", "EmployeeToTrips", nullable: true);

            migrationBuilder.Sql("Update [EmployeeToTrips] SET [IsApartmentNeeded]='true' WHERE [IsApartmentNeeded] IS null");

            migrationBuilder.AlterColumn<bool>("IsApartmentNeeded", "IsApartmentNeeded", nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
