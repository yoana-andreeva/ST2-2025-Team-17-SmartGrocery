using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrocery.Migrations
{
    /// <inheritdoc />
    public partial class DebugUserColumnCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ShoppingLists",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_UserId1",
                table: "ShoppingLists",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_AspNetUsers_UserId1",
                table: "ShoppingLists",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_AspNetUsers_UserId1",
                table: "ShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_UserId1",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ShoppingLists");
        }
    }
}
