using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Showcase",
                table: "Files",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase",
                table: "Files");
        }
    }
}
