using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkLab.Migrations
{
    public partial class RenamingProperty : Migration
    {
        // EF updated version is actually smart enough to do a rename now.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "People",
                newName: "MiddleNames");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MiddleNames",
                table: "People",
                newName: "MiddleName");
        }
    }
}
