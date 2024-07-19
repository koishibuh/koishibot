using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Migrations
{
    /// <inheritdoc />
    public partial class TwitchStreamIdToDandleResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TwitchStreamId",
                table: "DandleResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DandleResults_TwitchStreamId",
                table: "DandleResults",
                column: "TwitchStreamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DandleResults_TwitchStreams_TwitchStreamId",
                table: "DandleResults",
                column: "TwitchStreamId",
                principalTable: "TwitchStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DandleResults_TwitchStreams_TwitchStreamId",
                table: "DandleResults");

            migrationBuilder.DropIndex(
                name: "IX_DandleResults_TwitchStreamId",
                table: "DandleResults");

            migrationBuilder.DropColumn(
                name: "TwitchStreamId",
                table: "DandleResults");
        }
    }
}
