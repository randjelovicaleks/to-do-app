using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoInfrastructure.Migrations
{
    public partial class AddedOwnerAndRemovedUserEmailProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "ToDoLists");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "ToDoLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "ToDoLists");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "ToDoLists",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
