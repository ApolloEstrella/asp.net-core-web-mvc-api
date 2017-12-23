using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Login.Data.Migrations
{
    public partial class SingularizedFoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFoods_AspNetUsers_UserId",
                table: "UserFoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFoods",
                table: "UserFoods");

            migrationBuilder.RenameTable(
                name: "UserFoods",
                newName: "UserFood");

            migrationBuilder.RenameIndex(
                name: "IX_UserFoods_UserId",
                table: "UserFood",
                newName: "IX_UserFood_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFood",
                table: "UserFood",
                column: "UserFoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFood_AspNetUsers_UserId",
                table: "UserFood",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFood_AspNetUsers_UserId",
                table: "UserFood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFood",
                table: "UserFood");

            migrationBuilder.RenameTable(
                name: "UserFood",
                newName: "UserFoods");

            migrationBuilder.RenameIndex(
                name: "IX_UserFood_UserId",
                table: "UserFoods",
                newName: "IX_UserFoods_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFoods",
                table: "UserFoods",
                column: "UserFoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoods_AspNetUsers_UserId",
                table: "UserFoods",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
