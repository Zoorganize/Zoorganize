using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoorganize.Migrations
{
    /// <inheritdoc />
    public partial class ChnangeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "IdentificationTag",
                table: "Animals");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Animals",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Animals");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Animals",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationTag",
                table: "Animals",
                type: "TEXT",
                nullable: true);
        }
    }
}
