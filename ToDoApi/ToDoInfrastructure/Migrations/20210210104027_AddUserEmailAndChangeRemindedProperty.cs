using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoInfrastructure.Migrations
{
    public partial class AddUserEmailAndChangeRemindedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOpened",
                table: "ToDoLists");

            migrationBuilder.RenameColumn(
                name: "isReminded",
                table: "ToDoLists",
                newName: "IsReminded");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "ToDoLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "ToDoLists");

            migrationBuilder.RenameColumn(
                name: "IsReminded",
                table: "ToDoLists",
                newName: "isReminded");

            migrationBuilder.AddColumn<bool>(
                name: "isOpened",
                table: "ToDoLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
