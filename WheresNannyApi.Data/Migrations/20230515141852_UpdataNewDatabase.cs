using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class UpdataNewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nannys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicePrice = table.Column<double>(type: "float", nullable: false),
                    ApprovedToWork = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nannys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nannys_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    NannyId = table.Column<int>(type: "int", nullable: false),
                    HiringDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ServiceFinishHour = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Nannys_NannyId",
                        column: x => x.NannyId,
                        principalTable: "Nannys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Persons_PersonId",
                        column: x => x.PersonId,
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

            migrationBuilder.CreateIndex(
                name: "IX_Nannys_PersonId",
                table: "Nannys",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_NannyId",
                table: "Services",
                column: "NannyId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PersonId",
                table: "Services",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsRank");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Nannys");
        }
    }
}
