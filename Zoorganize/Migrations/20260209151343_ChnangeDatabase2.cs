using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoorganize.Migrations
{
    /// <inheritdoc />
    public partial class ChnangeDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalStaff");

            migrationBuilder.AddColumn<Guid>(
                name: "KeeperId",
                table: "Animals",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_KeeperId",
                table: "Animals",
                column: "KeeperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Staff_KeeperId",
                table: "Animals",
                column: "KeeperId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Staff_KeeperId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_KeeperId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "KeeperId",
                table: "Animals");

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

            migrationBuilder.CreateIndex(
                name: "IX_AnimalStaff_KeepersId",
                table: "AnimalStaff",
                column: "KeepersId");
        }
    }
}
