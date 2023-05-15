using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class RemoveLabelFromDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "Documents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Documents",
                type: "varchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
