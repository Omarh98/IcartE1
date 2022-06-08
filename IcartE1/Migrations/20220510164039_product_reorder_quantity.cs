using Microsoft.EntityFrameworkCore.Migrations;

namespace IcartE1.Migrations
{
    public partial class product_reorder_quantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReorderQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReorderQuantity",
                table: "Products");
        }
    }
}
