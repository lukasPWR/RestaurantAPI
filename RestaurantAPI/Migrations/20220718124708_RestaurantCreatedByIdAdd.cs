using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantAPI.Migrations
{
    public partial class RestaurantCreatedByIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatetdById",
                table: "Restaurants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatetdById",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
