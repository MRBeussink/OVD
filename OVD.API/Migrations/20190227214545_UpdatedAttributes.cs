using Microsoft.EntityFrameworkCore.Migrations;

namespace OVD.API.Migrations
{
    public partial class UpdatedAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "FakeUsers",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "FakeUsers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "FakeAdmins",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "FakeAdmins",
                newName: "Password");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "FakeUsers",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "FakeUsers",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "FakeAdmins",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "FakeAdmins",
                newName: "password");
        }
    }
}
