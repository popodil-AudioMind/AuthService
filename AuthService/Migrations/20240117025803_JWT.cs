using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class JWT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "salt",
                table: "LoginUsers",
                newName: "role");

            migrationBuilder.AddColumn<string>(
                name: "forumIDs",
                table: "LoginUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "forumIDs",
                table: "LoginUsers");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "LoginUsers",
                newName: "salt");
        }
    }
}
