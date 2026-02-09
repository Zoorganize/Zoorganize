using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoorganize.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    AreaInSquareMeters = table.Column<double>(type: "REAL", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommonName = table.Column<string>(type: "TEXT", nullable: false),
                    ScientificName = table.Column<string>(type: "TEXT", nullable: true),
                    MinAreaPerAnimal = table.Column<double>(type: "REAL", nullable: false),
                    MinGroupSize = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxGroupSize = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSolitaryByNature = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MaxTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MinHumidity = table.Column<double>(type: "REAL", nullable: true),
                    MaxHumidity = table.Column<double>(type: "REAL", nullable: true),
                    RequiresOutdoorAccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiredSecurityLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDangerous = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresSpecialPermit = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresWaterFeature = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresClimbingStructures = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresShelter = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Sex = table.Column<int>(type: "INTEGER", nullable: false),
                    JobRole = table.Column<int>(type: "INTEGER", nullable: false),
                    EmploymentType = table.Column<int>(type: "INTEGER", nullable: false),
                    YearlySalary = table.Column<float>(type: "REAL", nullable: false),
                    ContactInfo = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ExitDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zoos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalEnclosures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsOutdoor = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    TemperatureControlled = table.Column<bool>(type: "INTEGER", nullable: false),
                    MixedSpeciesAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MaxTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MinHumidity = table.Column<double>(type: "REAL", nullable: true),
                    MaxHumidity = table.Column<double>(type: "REAL", nullable: true),
                    IsEscapeProof = table.Column<bool>(type: "INTEGER", nullable: false),
                    SecurityLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    HasWaterFeature = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasClimbingStructures = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasShelter = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalEnclosures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalEnclosures_Rooms_Id",
                        column: x => x.Id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffRooms_Rooms_Id",
                        column: x => x.Id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OpeningHours = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorRooms_Rooms_Id",
                        column: x => x.Id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesStaff",
                columns: table => new
                {
                    AuthorizedSpeciesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StaffId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesStaff", x => new { x.AuthorizedSpeciesId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_SpeciesStaff_Species_AuthorizedSpeciesId",
                        column: x => x.AuthorizedSpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeciesStaff_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalEnclosureSpecies",
                columns: table => new
                {
                    AllowedSpeciesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimalEnclosureId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalEnclosureSpecies", x => new { x.AllowedSpeciesId, x.AnimalEnclosureId });
                    table.ForeignKey(
                        name: "FK_AnimalEnclosureSpecies_AnimalEnclosures_AnimalEnclosureId",
                        column: x => x.AnimalEnclosureId,
                        principalTable: "AnimalEnclosures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalEnclosureSpecies_Species_AllowedSpeciesId",
                        column: x => x.AllowedSpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ArrivalDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Origin = table.Column<int>(type: "INTEGER", nullable: false),
                    IdentificationTag = table.Column<string>(type: "TEXT", nullable: true),
                    Sex = table.Column<int>(type: "INTEGER", nullable: false),
                    IsNeutered = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPregnant = table.Column<bool>(type: "INTEGER", nullable: false),
                    HealthStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightKg = table.Column<double>(type: "REAL", nullable: true),
                    InQuarantine = table.Column<bool>(type: "INTEGER", nullable: false),
                    Aggressive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresSeparation = table.Column<bool>(type: "INTEGER", nullable: false),
                    BehavioralNotes = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentEnclosureId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EnclosureAssignedSince = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_AnimalEnclosures_CurrentEnclosureId",
                        column: x => x.CurrentEnclosureId,
                        principalTable: "AnimalEnclosures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Animals_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffStaffRooms",
                columns: table => new
                {
                    AuthorizedStaffId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StaffRoomsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffStaffRooms", x => new { x.AuthorizedStaffId, x.StaffRoomsId });
                    table.ForeignKey(
                        name: "FK_StaffStaffRooms_StaffRooms_StaffRoomsId",
                        column: x => x.StaffRoomsId,
                        principalTable: "StaffRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffStaffRooms_Staff_AuthorizedStaffId",
                        column: x => x.AuthorizedStaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffVisitorRoom",
                columns: table => new
                {
                    StaffId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VisitorRoomId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffVisitorRoom", x => new { x.StaffId, x.VisitorRoomId });
                    table.ForeignKey(
                        name: "FK_StaffVisitorRoom_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffVisitorRoom_VisitorRooms_VisitorRoomId",
                        column: x => x.VisitorRoomId,
                        principalTable: "VisitorRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalStaff",
                columns: table => new
                {
                    AssignedAnimalsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    KeepersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalStaff", x => new { x.AssignedAnimalsId, x.KeepersId });
                    table.ForeignKey(
                        name: "FK_AnimalStaff_Animals_AssignedAnimalsId",
                        column: x => x.AssignedAnimalsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalStaff_Staff_KeepersId",
                        column: x => x.KeepersId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalZooStays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ZooName = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalZooStays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalZooStays_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VeterinaryAppointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnimalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    AppointmentDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinaryAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeterinaryAppointments_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalEnclosureSpecies_AnimalEnclosureId",
                table: "AnimalEnclosureSpecies",
                column: "AnimalEnclosureId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_CurrentEnclosureId",
                table: "Animals",
                column: "CurrentEnclosureId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_SpeciesId",
                table: "Animals",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalStaff_KeepersId",
                table: "AnimalStaff",
                column: "KeepersId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalZooStays_AnimalId",
                table: "ExternalZooStays",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesStaff_StaffId",
                table: "SpeciesStaff",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffStaffRooms_StaffRoomsId",
                table: "StaffStaffRooms",
                column: "StaffRoomsId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffVisitorRoom_VisitorRoomId",
                table: "StaffVisitorRoom",
                column: "VisitorRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinaryAppointments_AnimalId",
                table: "VeterinaryAppointments",
                column: "AnimalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalEnclosureSpecies");

            migrationBuilder.DropTable(
                name: "AnimalStaff");

            migrationBuilder.DropTable(
                name: "ExternalZooStays");

            migrationBuilder.DropTable(
                name: "SpeciesStaff");

            migrationBuilder.DropTable(
                name: "StaffStaffRooms");

            migrationBuilder.DropTable(
                name: "StaffVisitorRoom");

            migrationBuilder.DropTable(
                name: "VeterinaryAppointments");

            migrationBuilder.DropTable(
                name: "Zoos");

            migrationBuilder.DropTable(
                name: "StaffRooms");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "VisitorRooms");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "AnimalEnclosures");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
