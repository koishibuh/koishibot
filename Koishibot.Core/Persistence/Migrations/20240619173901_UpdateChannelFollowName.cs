using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChannelFollowName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel Follow_Users_UserId",
                table: "Channel Follow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channel Follow",
                table: "Channel Follow");

            migrationBuilder.RenameTable(
                name: "Channel Follow",
                newName: "ChannelFollow");

            migrationBuilder.RenameIndex(
                name: "IX_Channel Follow_UserId",
                table: "ChannelFollow",
                newName: "IX_ChannelFollow_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelFollow",
                table: "ChannelFollow",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelFollow_Users_UserId",
                table: "ChannelFollow",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelFollow_Users_UserId",
                table: "ChannelFollow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelFollow",
                table: "ChannelFollow");

            migrationBuilder.RenameTable(
                name: "ChannelFollow",
                newName: "Channel Follow");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelFollow_UserId",
                table: "Channel Follow",
                newName: "IX_Channel Follow_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channel Follow",
                table: "Channel Follow",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel Follow_Users_UserId",
                table: "Channel Follow",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
