using Microsoft.EntityFrameworkCore.Migrations;

namespace IcartE1.Migrations
{
    public partial class batch_qr_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrImageUrl",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrImageUrl",
                table: "Batches");
        }
    }
}
