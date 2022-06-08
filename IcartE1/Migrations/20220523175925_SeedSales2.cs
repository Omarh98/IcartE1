using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcartE1.Migrations
{
    public partial class SeedSales2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                column: "Quantity",
                value: 53);

            migrationBuilder.UpdateData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                column: "Quantity",
                value: 65);

            migrationBuilder.UpdateData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                column: "Quantity",
                value: 72);

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "BranchId", "Date", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 100 },
                    { 1, new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 153 },
                    { 1, new DateTime(2022, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 265 },
                    { 1, new DateTime(2022, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 372 },
                    { 1, new DateTime(2022, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 200 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.DeleteData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.UpdateData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                column: "Quantity",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                column: "Quantity",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Sales",
                keyColumns: new[] { "BranchId", "Date", "ProductId" },
                keyValues: new object[] { 1, new DateTime(2022, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                column: "Quantity",
                value: 70);
        }
    }
}
