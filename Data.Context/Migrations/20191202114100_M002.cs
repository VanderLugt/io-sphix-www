using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Context.Migrations
{
    public partial class M002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailSentBox",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SentForId = table.Column<long>(nullable: false),
                    SentForTableName = table.Column<string>(maxLength: 100, nullable: true),
                    ToEMailId = table.Column<string>(maxLength: 500, nullable: true),
                    FromEMailId = table.Column<string>(maxLength: 100, nullable: true),
                    MessageType = table.Column<string>(maxLength: 100, nullable: true),
                    Subject = table.Column<string>(maxLength: 200, nullable: true),
                    Message = table.Column<string>(nullable: true),
                    SentDateTime = table.Column<DateTime>(nullable: false),
                    ReadDateTime = table.Column<DateTime>(nullable: false),
                    IsSent = table.Column<bool>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    SentBy = table.Column<long>(nullable: false),
                    SentToUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSentBox", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSentBox");
        }
    }
}
