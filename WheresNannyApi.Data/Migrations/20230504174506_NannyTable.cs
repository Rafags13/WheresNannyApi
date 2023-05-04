using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class NannyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Persons_personId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Persons_NannyId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsNanny",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "personId",
                table: "Addresses",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_personId",
                table: "Addresses",
                newName: "IX_Addresses_PersonId");

            migrationBuilder.CreateTable(
                name: "Nannys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ServicePrice = table.Column<double>(type: "float", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nannys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nannys_Persons_Id",
                        column: x => x.Id,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Nannys_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nannys_PersonId",
                table: "Nannys",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Persons_PersonId",
                table: "Addresses",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Nannys_NannyId",
                table: "Services",
                column: "NannyId",
                principalTable: "Nannys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Persons_PersonId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Nannys_NannyId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Nannys");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Addresses",
                newName: "personId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses",
                newName: "IX_Addresses_personId");

            migrationBuilder.AddColumn<bool>(
                name: "IsNanny",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Persons_personId",
                table: "Addresses",
                column: "personId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Persons_NannyId",
                table: "Services",
                column: "NannyId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
