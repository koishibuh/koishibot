using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommandNameRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandNames_ChatCommands_ChatCommandId",
                table: "CommandNames");

            migrationBuilder.AlterColumn<int>(
                name: "ChatCommandId",
                table: "CommandNames",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandNames_ChatCommands_ChatCommandId",
                table: "CommandNames",
                column: "ChatCommandId",
                principalTable: "ChatCommands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandNames_ChatCommands_ChatCommandId",
                table: "CommandNames");

            migrationBuilder.AlterColumn<int>(
                name: "ChatCommandId",
                table: "CommandNames",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommandNames_ChatCommands_ChatCommandId",
                table: "CommandNames",
                column: "ChatCommandId",
                principalTable: "ChatCommands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
