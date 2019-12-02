using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Context.Migrations
{
    public partial class M001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSentBox");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailSentBox",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromEMailId = table.Column<string>(maxLength: 100, nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    IsSent = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    MessageType = table.Column<string>(maxLength: 100, nullable: true),
                    ReadDateTime = table.Column<DateTime>(nullable: false),
                    SentBy = table.Column<long>(nullable: false),
                    SentDateTime = table.Column<DateTime>(nullable: false),
                    SentForId = table.Column<long>(nullable: false),
                    SentForTableName = table.Column<string>(maxLength: 100, nullable: true),
                    SentToUserId = table.Column<long>(nullable: false),
                    Subject = table.Column<string>(maxLength: 200, nullable: true),
                    ToEMailId = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSentBox", x => x.Id);
                });
        }
    }
}
