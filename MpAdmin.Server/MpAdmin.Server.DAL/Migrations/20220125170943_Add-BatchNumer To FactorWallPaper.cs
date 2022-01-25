using Microsoft.EntityFrameworkCore.Migrations;

namespace MpAdmin.Server.DAL.Migrations
{
    public partial class AddBatchNumerToFactorWallPaper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "FactorWallPapers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "FactorWallPapers");
        }
    }
}
