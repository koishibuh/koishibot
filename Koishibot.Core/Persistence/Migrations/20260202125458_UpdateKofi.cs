using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKofi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Kofi");

            migrationBuilder.AlterColumn<string>(
                name: "KofiType",
                table: "Kofi",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "AmountInPence",
                table: "Kofi",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountInPence",
                table: "Kofi");

            migrationBuilder.AlterColumn<int>(
                name: "KofiType",
                table: "Kofi",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "Kofi",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
