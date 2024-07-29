using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Migrations
{
    /// <inheritdoc />
    public partial class CommandGroupTimerAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Permissions = table.Column<int>(type: "int", nullable: false),
                    UserCooldown = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    GlobalCooldown = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatCommands", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TimerGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Interval = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimerGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommandName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChatCommandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandName_ChatCommands_ChatCommandId",
                        column: x => x.ChatCommandId,
                        principalTable: "ChatCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChatCommandTimerGroup",
                columns: table => new
                {
                    CommandsId = table.Column<int>(type: "int", nullable: false),
                    TimerGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatCommandTimerGroup", x => new { x.CommandsId, x.TimerGroupsId });
                    table.ForeignKey(
                        name: "FK_ChatCommandTimerGroup_ChatCommands_CommandsId",
                        column: x => x.CommandsId,
                        principalTable: "ChatCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatCommandTimerGroup_TimerGroups_TimerGroupsId",
                        column: x => x.TimerGroupsId,
                        principalTable: "TimerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChatCommandTimerGroup_TimerGroupsId",
                table: "ChatCommandTimerGroup",
                column: "TimerGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandName_ChatCommandId",
                table: "CommandName",
                column: "ChatCommandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatCommandTimerGroup");

            migrationBuilder.DropTable(
                name: "CommandName");

            migrationBuilder.DropTable(
                name: "TimerGroups");

            migrationBuilder.DropTable(
                name: "ChatCommands");
        }
    }
}
