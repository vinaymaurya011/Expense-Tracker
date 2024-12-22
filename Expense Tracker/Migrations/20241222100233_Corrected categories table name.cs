using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class Correctedcategoriestablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categoties_CategoryId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoties",
                table: "Categoties");

            migrationBuilder.RenameTable(
                name: "Categoties",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categoties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoties",
                table: "Categoties",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categoties_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categoties",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
