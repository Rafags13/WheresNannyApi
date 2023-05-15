using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class RemoveRequiredFieldFromIdPersonsAndNanny : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PersonWhoCommentId",
                table: "CommentsRank",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NannyWhoRecieveTheCommentId",
                table: "CommentsRank",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PersonWhoCommentId",
                table: "CommentsRank",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NannyWhoRecieveTheCommentId",
                table: "CommentsRank",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
