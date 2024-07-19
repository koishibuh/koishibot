using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koishibot.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChannelPointRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    TwitchId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    BackgroundColor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsUserInputRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsMaxPerStreamEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MaxPerStream = table.Column<int>(type: "int", nullable: false),
                    IsMaxPerUserPerStreamEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MaxPerUserPerStream = table.Column<int>(type: "int", nullable: false),
                    IsGlobalCooldownEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GlobalCooldownSeconds = table.Column<int>(type: "int", nullable: false),
                    IsPaused = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShouldRedemptionsSkipRequestQueue = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPointRewards", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TwitchId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "YearlyQuarters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyQuarters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateOnly>(type: "date", nullable: false),
                    AttendanceCount = table.Column<int>(type: "int", nullable: false),
                    StreakCurrentCount = table.Column<int>(type: "int", nullable: false),
                    StreakPersonalBest = table.Column<int>(type: "int", nullable: false),
                    StreakOptOut = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChannelPointRedemptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelPointRewardId = table.Column<int>(type: "int", nullable: false),
                    RedeemedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WasSuccesful = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPointRedemptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelPointRedemptions_ChannelPointRewards_ChannelPointRewa~",
                        column: x => x.ChannelPointRewardId,
                        principalTable: "ChannelPointRewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelPointRedemptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TwitchStreams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StreamId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    AttendanceMandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    YearlyQuarterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchStreams_YearlyQuarters_YearlyQuarterId",
                        column: x => x.YearlyQuarterId,
                        principalTable: "YearlyQuarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IncomingRaids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TwitchStreamId = table.Column<int>(type: "int", nullable: false),
                    RaidedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    RaidedByUserId = table.Column<int>(type: "int", nullable: false),
                    ViewerCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingRaids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingRaids_TwitchStreams_TwitchStreamId",
                        column: x => x.TwitchStreamId,
                        principalTable: "TwitchStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingRaids_Users_RaidedByUserId",
                        column: x => x.RaidedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OutgoingRaids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TwitchStreamId = table.Column<int>(type: "int", nullable: false),
                    RaidedUserId = table.Column<int>(type: "int", nullable: false),
                    SuggestedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingRaids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingRaids_TwitchStreams_TwitchStreamId",
                        column: x => x.TwitchStreamId,
                        principalTable: "TwitchStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingRaids_Users_RaidedUserId",
                        column: x => x.RaidedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutgoingRaids_Users_SuggestedByUserId",
                        column: x => x.SuggestedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TwitchStreamId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChoiceOne = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoteOne = table.Column<int>(type: "int", nullable: false),
                    ChoiceTwo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoteTwo = table.Column<int>(type: "int", nullable: false),
                    ChoiceThree = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoteThree = table.Column<int>(type: "int", nullable: false),
                    ChoiceFour = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoteFour = table.Column<int>(type: "int", nullable: false),
                    ChoiceFive = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoteFive = table.Column<int>(type: "int", nullable: false),
                    WinningChoice = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polls_TwitchStreams_TwitchStreamId",
                        column: x => x.TwitchStreamId,
                        principalTable: "TwitchStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UserId",
                table: "Attendances",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPointRedemptions_ChannelPointRewardId",
                table: "ChannelPointRedemptions",
                column: "ChannelPointRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPointRedemptions_UserId",
                table: "ChannelPointRedemptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPointRewards_TwitchId",
                table: "ChannelPointRewards",
                column: "TwitchId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingRaids_RaidedByUserId",
                table: "IncomingRaids",
                column: "RaidedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingRaids_TwitchStreamId",
                table: "IncomingRaids",
                column: "TwitchStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingRaids_RaidedUserId",
                table: "OutgoingRaids",
                column: "RaidedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingRaids_SuggestedByUserId",
                table: "OutgoingRaids",
                column: "SuggestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingRaids_TwitchStreamId",
                table: "OutgoingRaids",
                column: "TwitchStreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_TwitchStreamId",
                table: "Polls",
                column: "TwitchStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchStreams_StreamId",
                table: "TwitchStreams",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchStreams_YearlyQuarterId",
                table: "TwitchStreams",
                column: "YearlyQuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TwitchId",
                table: "Users",
                column: "TwitchId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "ChannelPointRedemptions");

            migrationBuilder.DropTable(
                name: "IncomingRaids");

            migrationBuilder.DropTable(
                name: "OutgoingRaids");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "ChannelPointRewards");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TwitchStreams");

            migrationBuilder.DropTable(
                name: "YearlyQuarters");
        }
    }
}
