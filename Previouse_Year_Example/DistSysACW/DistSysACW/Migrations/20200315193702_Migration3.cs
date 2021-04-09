using Microsoft.EntityFrameworkCore.Migrations;

namespace DistSysACW.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_logs_log_dataLogID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_log_dataLogID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "log_dataLogID",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Userapi_key",
                table: "logs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_logs_Userapi_key",
                table: "logs",
                column: "Userapi_key");

            migrationBuilder.AddForeignKey(
                name: "FK_logs_Users_Userapi_key",
                table: "logs",
                column: "Userapi_key",
                principalTable: "Users",
                principalColumn: "api_key",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logs_Users_Userapi_key",
                table: "logs");

            migrationBuilder.DropIndex(
                name: "IX_logs_Userapi_key",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "Userapi_key",
                table: "logs");

            migrationBuilder.AddColumn<int>(
                name: "log_dataLogID",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_log_dataLogID",
                table: "Users",
                column: "log_dataLogID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_logs_log_dataLogID",
                table: "Users",
                column: "log_dataLogID",
                principalTable: "logs",
                principalColumn: "LogID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
