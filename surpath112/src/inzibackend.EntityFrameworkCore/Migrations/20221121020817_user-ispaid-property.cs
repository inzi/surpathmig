using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class userispaidproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "AbpUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "AbpUsers");
        }
    }
}
