using Microsoft.EntityFrameworkCore.Migrations;

namespace MpAdmin.Server.DAL.Migrations
{
    public partial class AddWallPaperFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyPrice",
                table: "WallPapers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "WallPapers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "WallPapers");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "WallPapers");
        }
    }
}
