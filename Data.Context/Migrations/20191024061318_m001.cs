using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Context.Migrations
{
    public partial class m001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersAssociations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: true),
                    CommunityId = table.Column<int>(nullable: true),
                    AssociationId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersAssociations_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersAssociations_UsersLogin_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: true),
                    CommunityId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersGroups_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersGroups_UsersLogin_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersAssociations_CommunityId",
                table: "UsersAssociations",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAssociations_UserId",
                table: "UsersAssociations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGroups_CommunityId",
                table: "UsersGroups",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGroups_UserId",
                table: "UsersGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersAssociations");

            migrationBuilder.DropTable(
                name: "UsersGroups");
        }
    }
}
