using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChatCommandAndName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_ChatCommandId",
                table: "CommandTimerGroup");

            migrationBuilder.DropTable(
                name: "CommandNames");

            migrationBuilder.DropTable(
                name: "ChatCommands");

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
                name: "FK_CommandTimerGroup_NewChatCommand_NewChatCommandsId",
                table: "CommandTimerGroup",
                column: "NewChatCommandsId",
                principalTable: "NewChatCommand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandTimerGroup_NewChatCommand_NewChatCommandsId",
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

            migrationBuilder.CreateTable(
                name: "ChatCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GlobalCooldown = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Message = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Permissions = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserCooldown = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatCommands", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommandNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatCommandId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandNames_ChatCommands_ChatCommandId",
                        column: x => x.ChatCommandId,
                        principalTable: "ChatCommands",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommandNames_ChatCommandId",
                table: "CommandNames",
                column: "ChatCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandNames_Name",
                table: "CommandNames",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommandTimerGroup_ChatCommands_ChatCommandId",
                table: "CommandTimerGroup",
                column: "ChatCommandId",
                principalTable: "ChatCommands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
