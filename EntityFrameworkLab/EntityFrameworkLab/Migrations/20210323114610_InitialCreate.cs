using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkLab.Migrations
{
    // The file name prefix is the date and time of this change.
    // This file was created with the command Add-Migration in the package manager console.
    // Migration in DB is a similar concept to a local version control commit.
    // Push Up new changes, and revert back Down.
    // The Up() and Down() methods allow Entity Framework to revert and
    // reinstate changes that were made to the underlying database.
    // This migration file is the 'commit', stating what we are about to do to the actual database.
    // We execute this file with Update-Database command in the package manager console.
    // https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx
    public partial class InitialCreate : Migration
    {
        // First 'commit' to DB, so we can see creation of tables, columns and keys.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressIdentifier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseNameOrNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressIdentifier = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_People_Addresses_AddressIdentifier",
                        column: x => x.AddressIdentifier,
                        principalTable: "Addresses",
                        principalColumn: "AddressIdentifier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_AddressIdentifier",
                table: "People",
                column: "AddressIdentifier");
        }

        // We are about to create two new tables, so in case tables with this name already exist, drop them .
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
