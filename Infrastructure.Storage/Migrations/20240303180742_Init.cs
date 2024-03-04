using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserContexts",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ListName = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Command = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContexts", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "UserListInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActiveContext = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserListElements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserListInfoId = table.Column<long>(type: "INTEGER", nullable: false),
                    Number = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserListElements_UserListInfos_UserListInfoId",
                        column: x => x.UserListInfoId,
                        principalTable: "UserListInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserListElements_UserListInfoId",
                table: "UserListElements",
                column: "UserListInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListInfos_ChatId_Name",
                table: "UserListInfos",
                columns: new[] { "ChatId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContexts");

            migrationBuilder.DropTable(
                name: "UserListElements");

            migrationBuilder.DropTable(
                name: "UserListInfos");
        }
    }
}
