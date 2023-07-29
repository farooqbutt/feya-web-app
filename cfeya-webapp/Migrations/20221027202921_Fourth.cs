using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cfeya_webapp.Migrations
{
    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is18",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "61a4c488-0744-4b71-b665-fda1f18544ac");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8be91439-a430-45d1-876b-ec01ecd2967a");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is18",
                table: "User");

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
    }
}
