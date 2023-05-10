using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class CommentRankTableAndRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentsRank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "varchar(50)", nullable: true),
                    PersonWhoCommentId = table.Column<int>(type: "int", nullable: false),
                    NannyWhoRecieveTheCommentId = table.Column<int>(type: "int", nullable: false),
                    RankStarsCounting = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsRank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentsRank_Nannys_NannyWhoRecieveTheCommentId",
                        column: x => x.NannyWhoRecieveTheCommentId,
                        principalTable: "Nannys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentsRank_Persons_PersonWhoCommentId",
                        column: x => x.PersonWhoCommentId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentsRank_NannyWhoRecieveTheCommentId",
                table: "CommentsRank",
                column: "NannyWhoRecieveTheCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsRank_PersonWhoCommentId",
                table: "CommentsRank",
                column: "PersonWhoCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsRank");
        }
    }
}
