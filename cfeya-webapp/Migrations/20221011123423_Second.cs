using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cfeya_webapp.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WishID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "4880ba31-c917-45e5-851e-073c4e986beb");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "78314683-5e60-447b-8b88-e99aa4444984");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "243ebeac-69fb-45b0-ba78-e753ac045420");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "43b96182-bab4-499d-93d6-c6f5566bf7fa");
        }
    }
}
