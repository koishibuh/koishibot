using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandName");

            migrationBuilder.CreateTable(
                name: "CommandNames",
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
                    table.PrimaryKey("PK_CommandNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandNames_ChatCommands_ChatCommandId",
                        column: x => x.ChatCommandId,
                        principalTable: "ChatCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommandNames_ChatCommandId",
                table: "CommandNames",
                column: "ChatCommandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandNames");

            migrationBuilder.CreateTable(
                name: "CommandName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatCommandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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

            migrationBuilder.CreateIndex(
                name: "IX_CommandName_ChatCommandId",
                table: "CommandName",
                column: "ChatCommandId");
        }
    }
}
