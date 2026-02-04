using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gifted",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "Tier",
                table: "Subscriptions",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "EventMessage",
                table: "Subscriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserMessage",
                table: "Subscriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Timestamp",
                table: "Subscriptions",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Kofi_Timestamp",
                table: "Kofi",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Cheers_Timestamp",
                table: "Cheers",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_Timestamp",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Kofi_Timestamp",
                table: "Kofi");

            migrationBuilder.DropIndex(
                name: "IX_Cheers_Timestamp",
                table: "Cheers");

            migrationBuilder.DropColumn(
                name: "EventMessage",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UserMessage",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Subscriptions",
                newName: "Tier");

            migrationBuilder.AddColumn<bool>(
                name: "Gifted",
                table: "Subscriptions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
