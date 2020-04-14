using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.Data.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageUris_Products_ProductId",
                table: "ImageUris");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Distributors_DistributorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DistributorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ImageUris_ProductId",
                table: "ImageUris");

            migrationBuilder.AlterColumn<int>(
                name: "DistributorId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DistributorId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Products_DistributorId",
                table: "Products",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageUris_ProductId",
                table: "ImageUris",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageUris_Products_ProductId",
                table: "ImageUris",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Distributors_DistributorId",
                table: "Products",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
