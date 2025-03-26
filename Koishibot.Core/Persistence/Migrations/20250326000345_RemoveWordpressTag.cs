using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWordpressTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KoiKinDragons_WordpressItemTags_ItemTagId",
                table: "KoiKinDragons");

            migrationBuilder.DropTable(
                name: "WordpressItemTags");

            migrationBuilder.DropIndex(
                name: "IX_KoiKinDragons_ItemTagId",
                table: "KoiKinDragons");

            migrationBuilder.DropColumn(
                name: "ItemTagId",
                table: "KoiKinDragons");

            migrationBuilder.DropColumn(
                name: "WordpressId",
                table: "KoiKinDragons");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "KoiKinDragons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "KoiKinDragons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ItemTagId",
                table: "KoiKinDragons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WordpressId",
                table: "KoiKinDragons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WordpressItemTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    WordPressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordpressItemTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordpressItemTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_KoiKinDragons_ItemTagId",
                table: "KoiKinDragons",
                column: "ItemTagId");

            migrationBuilder.CreateIndex(
                name: "IX_WordpressItemTags_UserId",
                table: "WordpressItemTags",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiKinDragons_WordpressItemTags_ItemTagId",
                table: "KoiKinDragons",
                column: "ItemTagId",
                principalTable: "WordpressItemTags",
                principalColumn: "Id");
        }
    }
}
