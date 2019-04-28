using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    OfficeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.OfficeID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    ApartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    RoomNumber = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    OfficeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.ApartmentID);
                    table.ForeignKey(
                        name: "FK_Apartments_Offices_OfficeID",
                        column: x => x.OfficeID,
                        principalTable: "Offices",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartureDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsPlaneNeeded = table.Column<bool>(nullable: false),
                    IsCarRentalNeeded = table.Column<bool>(nullable: false),
                    IsCarCompensationNeeded = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    DepartureOfficeID = table.Column<int>(nullable: false),
                    ArrivalOfficeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripID);
                    table.ForeignKey(
                        name: "FK_Trips_Offices_ArrivalOfficeID",
                        column: x => x.ArrivalOfficeID,
                        principalTable: "Offices",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Trips_Offices_DepartureOfficeID",
                        column: x => x.DepartureOfficeID,
                        principalTable: "Offices",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CarRentals",
                columns: table => new
                {
                    CarRentalID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarRentalCompany = table.Column<string>(nullable: true),
                    CarPickupAddress = table.Column<string>(nullable: true),
                    CarIssueDate = table.Column<DateTime>(nullable: false),
                    CarReturnDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    CarRentalUrl = table.Column<string>(nullable: true),
                    TripID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRentals", x => x.CarRentalID);
                    table.ForeignKey(
                        name: "FK_CarRentals_Trips_TripID",
                        column: x => x.TripID,
                        principalTable: "Trips",
                        principalColumn: "TripID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeToTrips",
                columns: table => new
                {
                    EmployeeToTripID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true),
                    WasRead = table.Column<bool>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: false),
                    TripId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeToTrips", x => x.EmployeeToTripID);
                    table.ForeignKey(
                        name: "FK_EmployeeToTrips_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeToTrips_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasCompensations",
                columns: table => new
                {
                    GasCompensationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Price = table.Column<int>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: false),
                    TripID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasCompensations", x => x.GasCompensationID);
                    table.ForeignKey(
                        name: "FK_GasCompensations_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GasCompensations_Trips_TripID",
                        column: x => x.TripID,
                        principalTable: "Trips",
                        principalColumn: "TripID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaneTickets",
                columns: table => new
                {
                    PlaneTicketID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlightCompany = table.Column<string>(nullable: true),
                    Airport = table.Column<string>(nullable: true),
                    ForwardFlightDate = table.Column<DateTime>(nullable: false),
                    ReturnFlightDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    PlaneTicketUrl = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: false),
                    TripID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneTickets", x => x.PlaneTicketID);
                    table.ForeignKey(
                        name: "FK_PlaneTickets_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaneTickets_Trips_TripID",
                        column: x => x.TripID,
                        principalTable: "Trips",
                        principalColumn: "TripID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckIn = table.Column<DateTime>(nullable: false),
                    CheckOut = table.Column<DateTime>(nullable: false),
                    ReservationUrl = table.Column<string>(nullable: true),
                    ApartmentID = table.Column<int>(nullable: false),
                    TripID = table.Column<int>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservations_Apartments_ApartmentID",
                        column: x => x.ApartmentID,
                        principalTable: "Apartments",
                        principalColumn: "ApartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Trips_TripID",
                        column: x => x.TripID,
                        principalTable: "Trips",
                        principalColumn: "TripID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_OfficeID",
                table: "Apartments",
                column: "OfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_TripID",
                table: "CarRentals",
                column: "TripID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeToTrips_EmployeeID",
                table: "EmployeeToTrips",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeToTrips_TripId",
                table: "EmployeeToTrips",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmployeeID",
                table: "Events",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_GasCompensations_EmployeeID",
                table: "GasCompensations",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_GasCompensations_TripID",
                table: "GasCompensations",
                column: "TripID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaneTickets_EmployeeID",
                table: "PlaneTickets",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaneTickets_TripID",
                table: "PlaneTickets",
                column: "TripID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ApartmentID",
                table: "Reservations",
                column: "ApartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EmployeeID",
                table: "Reservations",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TripID",
                table: "Reservations",
                column: "TripID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ArrivalOfficeID",
                table: "Trips",
                column: "ArrivalOfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DepartureOfficeID",
                table: "Trips",
                column: "DepartureOfficeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarRentals");

            migrationBuilder.DropTable(
                name: "EmployeeToTrips");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "GasCompensations");

            migrationBuilder.DropTable(
                name: "PlaneTickets");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
