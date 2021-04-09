using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistSysACW.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    LogID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Log_string = table.Column<string>(nullable: true),
                    LogDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    api_key = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    user_name = table.Column<string>(nullable: true),
                    log_dataLogID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.api_key);
                    table.ForeignKey(
                        name: "FK_Users_logs_log_dataLogID",
                        column: x => x.log_dataLogID,
                        principalTable: "logs",
                        principalColumn: "LogID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_log_dataLogID",
                table: "Users",
                column: "log_dataLogID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "logs");
        }
    }
}
