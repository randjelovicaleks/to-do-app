using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoInfrastructure.Migrations
{
    public partial class PropertyChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ToDoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "ToDoLists");
        }
    }
}
