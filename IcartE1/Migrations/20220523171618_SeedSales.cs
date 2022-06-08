using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcartE1.Migrations
{
    public partial class SeedSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "BranchId", "Date", "ProductId", "Quantity" },
                values: new object[] { 1, new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 50 });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "BranchId", "Date", "ProductId", "Quantity" },
                values: new object[] { 1, new DateTime(2022, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 60 });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "BranchId", "Date", "ProductId", "Quantity" },
                values: new object[] { 1, new DateTime(2022, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 70 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });
        }
    }
}
