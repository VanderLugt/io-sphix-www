using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Context.Migrations
{
    public partial class M004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerificationTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    TokenUsedOn = table.Column<DateTime>(nullable: false),
                    TokenForId = table.Column<long>(nullable: false),
                    TokenForTableName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationTokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationTokens");
        }
    }
}
