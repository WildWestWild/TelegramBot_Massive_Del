using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class StrikingOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStrikingOut",
                table: "UserListElements",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStrikingOut",
                table: "UserListElements");
        }
    }
}
