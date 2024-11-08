using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.WebApiRestful.Data.Migrations
{
    /// <inheritdoc />
    public partial class addnewColusertoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeRefreshToken",
                table: "UserToken",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeRefreshToken",
                table: "UserToken");
        }
    }
}
