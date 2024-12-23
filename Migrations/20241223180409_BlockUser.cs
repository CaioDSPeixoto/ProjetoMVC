using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMvc.Migrations
{
    /// <inheritdoc />
    public partial class BlockUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_User_AssignedToUserId",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_User_CreatedByUserId",
                table: "Todos");

            migrationBuilder.AddColumn<int>(
                name: "BlockedById",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedUntil",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_BlockedById",
                table: "User",
                column: "BlockedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_User_AssignedToUserId",
                table: "Todos",
                column: "AssignedToUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_User_CreatedByUserId",
                table: "Todos",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_BlockedById",
                table: "User",
                column: "BlockedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_BlockedById",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_BlockedById",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BlockedById",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BlockedUntil",
                table: "User");

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
    }
}
