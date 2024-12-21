using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsOnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Todo_CategoryId",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todo",
                table: "Todo");

            migrationBuilder.RenameTable(
                name: "Todo",
                newName: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Category_CategoryId",
                table: "Todos",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Category_CategoryId",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Todo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todo",
                table: "Todo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Todo_CategoryId",
                table: "Todos",
                column: "CategoryId",
                principalTable: "Todo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
