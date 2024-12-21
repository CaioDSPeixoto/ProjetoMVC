using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLinksToTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "Todos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_AssignedToUserId",
                table: "Todos",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CreatedByUserId",
                table: "Todos",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_User_AssignedToUserId",
                table: "Todos",
                column: "AssignedToUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_User_CreatedByUserId",
                table: "Todos",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_User_AssignedToUserId",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_User_CreatedByUserId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_AssignedToUserId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_CreatedByUserId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
