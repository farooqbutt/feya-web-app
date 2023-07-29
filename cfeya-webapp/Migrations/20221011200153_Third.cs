using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cfeya_webapp.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MarkasRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ac68e175-8038-4d58-a727-6d0657933d00");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "70ccc120-fec9-41ef-a05f-7390c042c5e9");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkasRead",
                table: "Notifications");

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
        }
    }
}
