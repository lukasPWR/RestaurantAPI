using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantAPI.Migrations
{
    public partial class DishesCreatedByIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CreatedById",
                table: "Dishes",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Users_CreatedById",
                table: "Dishes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Users_CreatedById",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CreatedById",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Dishes");
        }
    }
}
