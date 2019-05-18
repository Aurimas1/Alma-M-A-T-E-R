using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ModelFixes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("Currency", "Apartments", nullable: true);
            migrationBuilder.AddColumn<string>("Currency", "CarRentals", nullable: true);
            migrationBuilder.AddColumn<string>("Currency", "GasCompensations", nullable: true);
            migrationBuilder.AddColumn<string>("Currency", "PlaneTickets", nullable: true);

            migrationBuilder.Sql("Update [Apartments] SET [Currency]='EUR' WHERE [Currency] IS null");
            migrationBuilder.Sql("Update [CarRentals] SET [Currency]='EUR' WHERE [Currency] IS null");
            migrationBuilder.Sql("Update [GasCompensations] SET [Currency]='EUR' WHERE [Currency] IS null");
            migrationBuilder.Sql("Update [PlaneTickets] SET [Currency]='EUR' WHERE [Currency] IS null");

            migrationBuilder.AlterColumn<string>("Currency", "Apartments", nullable: false);
            migrationBuilder.AlterColumn<string>("Currency", "CarRentals", nullable: false);
            migrationBuilder.AlterColumn<string>("Currency", "GasCompensations", nullable: false);
            migrationBuilder.AlterColumn<string>("Currency", "PlaneTickets", nullable: false);

            migrationBuilder.AddColumn<int>("OrganizerID", "Trips", nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
