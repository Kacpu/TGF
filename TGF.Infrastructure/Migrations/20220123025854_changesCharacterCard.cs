using Microsoft.EntityFrameworkCore.Migrations;

namespace TGF.Infrastructure.Migrations
{
    public partial class changesCharacterCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterCards_Characters_CharacterId",
                table: "CharacterCards");

            migrationBuilder.DropIndex(
                name: "IX_CharacterCards_CharacterId",
                table: "CharacterCards");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "CharacterCards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCards_CharacterId",
                table: "CharacterCards",
                column: "CharacterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterCards_Characters_CharacterId",
                table: "CharacterCards",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterCards_Characters_CharacterId",
                table: "CharacterCards");

            migrationBuilder.DropIndex(
                name: "IX_CharacterCards_CharacterId",
                table: "CharacterCards");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "CharacterCards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCards_CharacterId",
                table: "CharacterCards",
                column: "CharacterId",
                unique: true,
                filter: "[CharacterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterCards_Characters_CharacterId",
                table: "CharacterCards",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
