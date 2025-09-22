using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_NewChatCommandsId",
                table: "CommandTimerGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommandTimerGroup",
                table: "CommandTimerGroup");

            migrationBuilder.DropColumn(
                name: "NewChatCommandsId",
                table: "CommandTimerGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommandTimerGroup",
                table: "CommandTimerGroup",
                columns: new[] { "ChatCommandId", "TimerGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_ChatCommandId",
                table: "CommandTimerGroup",
                column: "ChatCommandId",
                principalTable: "ChatCommands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_ChatCommandId",
                table: "CommandTimerGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommandTimerGroup",
                table: "CommandTimerGroup");

            migrationBuilder.AddColumn<int>(
                name: "NewChatCommandsId",
                table: "CommandTimerGroup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommandTimerGroup",
                table: "CommandTimerGroup",
                columns: new[] { "NewChatCommandsId", "TimerGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_NewChatCommandsId",
                table: "CommandTimerGroup",
                column: "NewChatCommandsId",
                principalTable: "ChatCommands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
