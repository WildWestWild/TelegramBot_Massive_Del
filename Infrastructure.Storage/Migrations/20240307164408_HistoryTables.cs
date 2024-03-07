using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class HistoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveContext",
                table: "UserListInfos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserContexts");

            migrationBuilder.CreateTable(
                name: "UserListHistories",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    ListName = table.Column<string>(type: "TEXT", nullable: false),
                    LastUseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListHistories", x => new { x.ChatId, x.ListName });
                });

            migrationBuilder.CreateTable(
                name: "UserListHistoryPointers",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OffSet = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListHistoryPointers", x => x.ChatId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserListHistories");

            migrationBuilder.DropTable(
                name: "UserListHistoryPointers");

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveContext",
                table: "UserListInfos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserContexts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
