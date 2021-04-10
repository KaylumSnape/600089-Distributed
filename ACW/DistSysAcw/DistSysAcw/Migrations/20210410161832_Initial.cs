using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistSysAcw.Migrations
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

    // Need EF tool package. Look at packages in this project, I had to install 3.
    // Use Package Manager Console (View -> Other Windows -> Package Manager Console).
    // In that console, create migration file using the command Add-Migration <GiveAName>.
    // This is built from our UserContext class (It finds it for us based of naming conventions).
    // Acts like a commit, showing us what it plans to do to our database.
    // Then perform the changes using the command Update-Database.
    // -Verbose flag for more information.
    // View - SQL Server Object Explorer - To see our databases.
    // Right click > View data on a table. Hit refresh arrow to update view (or reopen).
    // When updating the objects/classes in code, we must get entity framework to update our
    // database, so that they are both in sync. This is done through Migrations.
    // In order to update the database to match our model we’ll have to make
    // a new migration that has the changes.
    // .dbo MigrationsHistory table allows you to rollback & apply multiple migrations.
    // When renaming a property in your model/class, the column in the DB must also be updated.
    // Do a Migration, but edit the migration file before updating,
    // EF isn't smart enough and wants to drop the old column and data and then add a new one in place.
    // We don't want to lose our existing data.
    // New version of EF is smart enough, so this isn't an issue anymore.
    // It's still good to check the migration file before 'pushing' your changes to the DB.

    // Update-Database <Migration_Name> to rollback.
    // If you want to start clean, manually delete migrations and tables in sql explorer.
    // https://stackoverflow.com/questions/16035333/how-to-delete-and-recreate-from-scratch-an-existing-ef-code-first-database
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogArchives",
                columns: table => new
                {
                    LogArchiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<int>(type: "int", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogString = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    LogDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogArchives", x => x.LogArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ApiKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ApiKey);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogString = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    LogDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserApiKey = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Logs_Users_UserApiKey",
                        column: x => x.UserApiKey,
                        principalTable: "Users",
                        principalColumn: "ApiKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserApiKey",
                table: "Logs",
                column: "UserApiKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogArchives");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
