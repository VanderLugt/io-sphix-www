using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Context.Migrations
{
    public partial class m002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublicGroup",
                table: "UserCommunitiesGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublicGroup",
                table: "UserCommunitiesGroups");
        }
    }
}
