using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class HiringTimeService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Services",
                newName: "ServiceFinishHour");

            migrationBuilder.AddColumn<DateTime>(
                name: "HiringDate",
                table: "Services",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "RankAvegerageStars",
                table: "Nannys",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiringDate",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "RankAvegerageStars",
                table: "Nannys");

            migrationBuilder.RenameColumn(
                name: "ServiceFinishHour",
                table: "Services",
                newName: "Time");
        }
    }
}
