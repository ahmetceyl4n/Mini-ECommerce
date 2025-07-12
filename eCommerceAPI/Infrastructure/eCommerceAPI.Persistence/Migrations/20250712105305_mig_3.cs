using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Products_ProductID",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ProductID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "ProductProductImageFile",
                columns: table => new
                {
                    ProductsID = table.Column<Guid>(type: "uuid", nullable: false),
                    productImageFilesID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductImageFile", x => new { x.ProductsID, x.productImageFilesID });
                    table.ForeignKey(
                        name: "FK_ProductProductImageFile_Files_productImageFilesID",
                        column: x => x.productImageFilesID,
                        principalTable: "Files",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductImageFile_Products_ProductsID",
                        column: x => x.ProductsID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductImageFile_productImageFilesID",
                table: "ProductProductImageFile",
                column: "productImageFilesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductImageFile");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductID",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProductID",
                table: "Files",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Products_ProductID",
                table: "Files",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
