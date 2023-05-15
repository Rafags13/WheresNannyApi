using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class NannyApprovedToWorkResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApprovedToWork",
                table: "Nannys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedToWork",
                table: "Nannys");
        }
    }
}
