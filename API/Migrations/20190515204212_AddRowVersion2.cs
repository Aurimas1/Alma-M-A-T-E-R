using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddRowVersion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>("RowVersion", "Apartments", rowVersion: true, nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
