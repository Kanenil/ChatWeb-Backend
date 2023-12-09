using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatWeb.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFileNamefieldtotblMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "tblMessages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "tblMessages");
        }
    }
}
