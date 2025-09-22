using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCommandTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandTimerGroup_NewChatCommand_NewChatCommandsId",
                table: "CommandTimerGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_NewChatCommand_ChatResponse_ResponseId",
                table: "NewChatCommand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewChatCommand",
                table: "NewChatCommand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatResponse",
                table: "ChatResponse");

            migrationBuilder.RenameTable(
                name: "NewChatCommand",
                newName: "ChatCommands");

            migrationBuilder.RenameTable(
                name: "ChatResponse",
                newName: "ChatResponses");

            migrationBuilder.RenameIndex(
                name: "IX_NewChatCommand_ResponseId",
                table: "ChatCommands",
                newName: "IX_ChatCommands_ResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_NewChatCommand_Name",
                table: "ChatCommands",
                newName: "IX_ChatCommands_Name");

            migrationBuilder.RenameIndex(
                name: "IX_ChatResponse_InternalName",
                table: "ChatResponses",
                newName: "IX_ChatResponses_InternalName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatCommands",
                table: "ChatCommands",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatResponses",
                table: "ChatResponses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatCommands_ChatResponses_ResponseId",
                table: "ChatCommands",
                column: "ResponseId",
                principalTable: "ChatResponses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_NewChatCommandsId",
                table: "CommandTimerGroup",
                column: "NewChatCommandsId",
                principalTable: "ChatCommands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatCommands_ChatResponses_ResponseId",
                table: "ChatCommands");

            migrationBuilder.DropForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_NewChatCommandsId",
                table: "CommandTimerGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatResponses",
                table: "ChatResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatCommands",
                table: "ChatCommands");

            migrationBuilder.RenameTable(
                name: "ChatResponses",
                newName: "ChatResponse");

            migrationBuilder.RenameTable(
                name: "ChatCommands",
                newName: "NewChatCommand");

            migrationBuilder.RenameIndex(
                name: "IX_ChatResponses_InternalName",
                table: "ChatResponse",
                newName: "IX_ChatResponse_InternalName");

            migrationBuilder.RenameIndex(
                name: "IX_ChatCommands_ResponseId",
                table: "NewChatCommand",
                newName: "IX_NewChatCommand_ResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatCommands_Name",
                table: "NewChatCommand",
                newName: "IX_NewChatCommand_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatResponse",
                table: "ChatResponse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewChatCommand",
                table: "NewChatCommand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandTimerGroup_NewChatCommand_NewChatCommandsId",
                table: "CommandTimerGroup",
                column: "NewChatCommandsId",
                principalTable: "NewChatCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewChatCommand_ChatResponse_ResponseId",
                table: "NewChatCommand",
                column: "ResponseId",
                principalTable: "ChatResponse",
                principalColumn: "Id");
        }
    }
}
