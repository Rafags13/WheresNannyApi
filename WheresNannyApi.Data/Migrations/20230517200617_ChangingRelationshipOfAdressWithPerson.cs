using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheresNannyApi.Data.Migrations
{
    public partial class ChangingRelationshipOfAdressWithPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "PersonId", table: "Addresses");
            migrationBuilder.DropForeignKey(name: "FK_Addresses_Persons_PersonId", table: "Addresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "PersonId", table: "Addresses");
            migrationBuilder.DropForeignKey(name: "FK_Addresses_Persons_PersonId", table: "Addresses");
        }
    }
}
