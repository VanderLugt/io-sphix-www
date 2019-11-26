using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Context.Migrations
{
    public partial class M003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "UserJoinEventMeetings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "UserJoinCommunityOpenHourMeetings",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCommunityOpenOfficeHoursMeetingsStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MeetingId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    MeetingStatus = table.Column<string>(maxLength: 20, nullable: false),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommunityOpenOfficeHoursMeetingsStatus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCommunityOpenOfficeHoursMeetingsStatus");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "UserJoinEventMeetings");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "UserJoinCommunityOpenHourMeetings");
        }
    }
}
