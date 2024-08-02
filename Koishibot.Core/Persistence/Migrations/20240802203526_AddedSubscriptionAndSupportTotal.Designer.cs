﻿// <auto-generated />
using System;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Koishibot.Core.Migrations
{
    [DbContext(typeof(KoishibotDbContext))]
    [Migration("20240802203526_AddedSubscriptionAndSupportTotal")]
    partial class AddedSubscriptionAndSupportTotal
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ChatCommandTimerGroup", b =>
                {
                    b.Property<int>("CommandsId")
                        .HasColumnType("int");

                    b.Property<int>("TimerGroupsId")
                        .HasColumnType("int");

                    b.HasKey("CommandsId", "TimerGroupsId");

                    b.HasIndex("TimerGroupsId");

                    b.ToTable("ChatCommandTimerGroup");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ApplicationAuthentication.Models.AppLogin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AppLogin", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.AttendanceLog.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttendanceCount")
                        .HasColumnType("int");

                    b.Property<DateOnly>("LastUpdated")
                        .HasColumnType("date");

                    b.Property<int>("StreakCurrentCount")
                        .HasColumnType("int");

                    b.Property<bool>("StreakOptOut")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("StreakPersonalBest")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Attendances", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChannelPoints.Models.ChannelPointRedemption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChannelPointRewardId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("RedeemedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("WasSuccesful")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelPointRewardId");

                    b.HasIndex("UserId");

                    b.ToTable("ChannelPointRedemptions", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChannelPoints.Models.ChannelPointReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BackgroundColor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("GlobalCooldownSeconds")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsGlobalCooldownEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMaxPerStreamEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMaxPerUserPerStreamEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPaused")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsUserInputRequired")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaxPerStream")
                        .HasColumnType("int");

                    b.Property<int>("MaxPerUserPerStream")
                        .HasColumnType("int");

                    b.Property<bool>("ShouldRedemptionsSkipRequestQueue")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TwitchId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("TwitchId");

                    b.ToTable("ChannelPointRewards", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChatCommands.Models.ChatCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<TimeSpan>("GlobalCooldown")
                        .HasColumnType("time(6)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Permissions")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("UserCooldown")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("ChatCommands");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChatCommands.Models.CommandName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ChatCommandId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ChatCommandId");

                    b.ToTable("CommandNames");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChatCommands.Models.TimerGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("Interval")
                        .HasColumnType("time(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TimerGroups");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Dandle.Models.DandleResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DandleWordId")
                        .HasColumnType("int");

                    b.Property<int>("GuessCount")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TwitchStreamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DandleWordId");

                    b.HasIndex("TwitchStreamId")
                        .IsUnique();

                    b.ToTable("DandleResults", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Dandle.Models.DandleWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("Word")
                        .IsUnique();

                    b.ToTable("DandleWords", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Polls.Models.PollResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ChoiceFive")
                        .HasColumnType("longtext");

                    b.Property<string>("ChoiceFour")
                        .HasColumnType("longtext");

                    b.Property<string>("ChoiceOne")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ChoiceThree")
                        .HasColumnType("longtext");

                    b.Property<string>("ChoiceTwo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("StartedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TwitchStreamId")
                        .HasColumnType("int");

                    b.Property<int>("VoteFive")
                        .HasColumnType("int");

                    b.Property<int>("VoteFour")
                        .HasColumnType("int");

                    b.Property<int>("VoteOne")
                        .HasColumnType("int");

                    b.Property<int>("VoteThree")
                        .HasColumnType("int");

                    b.Property<int>("VoteTwo")
                        .HasColumnType("int");

                    b.Property<string>("WinningChoice")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("TwitchStreamId");

                    b.ToTable("Polls", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.RaidSuggestions.Models.OutgoingRaid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RaidedUserId")
                        .HasColumnType("int");

                    b.Property<int>("SuggestedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("TwitchStreamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RaidedUserId");

                    b.HasIndex("SuggestedByUserId");

                    b.HasIndex("TwitchStreamId")
                        .IsUnique();

                    b.ToTable("OutgoingRaids", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Raids.Models.IncomingRaid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("RaidedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RaidedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("TwitchStreamId")
                        .HasColumnType("int");

                    b.Property<int>("ViewerCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RaidedByUserId");

                    b.HasIndex("TwitchStreamId");

                    b.ToTable("IncomingRaids", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AttendanceMandatory")
                        .HasColumnType("tinyint(1)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time(6)");

                    b.Property<DateTimeOffset>("StartedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StreamId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("YearlyQuarterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StreamId")
                        .IsUnique();

                    b.HasIndex("YearlyQuarterId");

                    b.ToTable("TwitchStreams", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.StreamInformation.Models.YearlyQuarter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("YearlyQuarters", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.ChannelFollow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ChannelFollow", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.GiftSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Tier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GiftSubscriptions", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Gifted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<string>("Tier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.SupportTotal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BitsCheered")
                        .HasColumnType("int");

                    b.Property<int>("MonthsSubscribed")
                        .HasColumnType("int");

                    b.Property<int>("SubsGifted")
                        .HasColumnType("int");

                    b.Property<int>("Tipped")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("SupportTotals", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.TwitchCheer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BitsAmount")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cheers", (string)null);
                });

            modelBuilder.Entity("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TwitchId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("TwitchId")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ChatCommandTimerGroup", b =>
                {
                    b.HasOne("Koishibot.Core.Features.ChatCommands.Models.ChatCommand", null)
                        .WithMany()
                        .HasForeignKey("CommandsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Koishibot.Core.Features.ChatCommands.Models.TimerGroup", null)
                        .WithMany()
                        .HasForeignKey("TimerGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Koishibot.Core.Features.AttendanceLog.Models.Attendance", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "User")
                        .WithOne("Attendance")
                        .HasForeignKey("Koishibot.Core.Features.AttendanceLog.Models.Attendance", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChannelPoints.Models.ChannelPointRedemption", b =>
                {
                    b.HasOne("Koishibot.Core.Features.ChannelPoints.Models.ChannelPointReward", "ChannelPointReward")
                        .WithMany("ChannelPointRedemptions")
                        .HasForeignKey("ChannelPointRewardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "User")
                        .WithMany("RedeemedChannelPointRewards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelPointReward");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChatCommands.Models.CommandName", b =>
                {
                    b.HasOne("Koishibot.Core.Features.ChatCommands.Models.ChatCommand", "ChatCommand")
                        .WithMany("CommandNames")
                        .HasForeignKey("ChatCommandId");

                    b.Navigation("ChatCommand");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Dandle.Models.DandleResult", b =>
                {
                    b.HasOne("Koishibot.Core.Features.Dandle.Models.DandleWord", "DandleWord")
                        .WithMany("DandleResults")
                        .HasForeignKey("DandleWordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", "TwitchStream")
                        .WithOne("DandleResult")
                        .HasForeignKey("Koishibot.Core.Features.Dandle.Models.DandleResult", "TwitchStreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DandleWord");

                    b.Navigation("TwitchStream");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Polls.Models.PollResult", b =>
                {
                    b.HasOne("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", "TwitchStream")
                        .WithMany("PollResults")
                        .HasForeignKey("TwitchStreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchStream");
                });

            modelBuilder.Entity("Koishibot.Core.Features.RaidSuggestions.Models.OutgoingRaid", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "RaidedUser")
                        .WithMany("UsersSuggestingThisRaidTarget")
                        .HasForeignKey("RaidedUserId")
                        .IsRequired();

                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "SuggestedByUser")
                        .WithMany("RaidsSuggestedByThisUser")
                        .HasForeignKey("SuggestedByUserId")
                        .IsRequired();

                    b.HasOne("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", "TwitchStream")
                        .WithOne("OutgoingRaid")
                        .HasForeignKey("Koishibot.Core.Features.RaidSuggestions.Models.OutgoingRaid", "TwitchStreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RaidedUser");

                    b.Navigation("SuggestedByUser");

                    b.Navigation("TwitchStream");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Raids.Models.IncomingRaid", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "RaidedByUser")
                        .WithMany("RaidsFromThisUser")
                        .HasForeignKey("RaidedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", "TwitchStream")
                        .WithMany("IncomingRaids")
                        .HasForeignKey("TwitchStreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RaidedByUser");

                    b.Navigation("TwitchStream");
                });

            modelBuilder.Entity("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", b =>
                {
                    b.HasOne("Koishibot.Core.Features.StreamInformation.Models.YearlyQuarter", "YearlyQuarter")
                        .WithMany("TwitchStreams")
                        .HasForeignKey("YearlyQuarterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("YearlyQuarter");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.ChannelFollow", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "TwitchUser")
                        .WithMany("ChannelFollows")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchUser");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.GiftSubscription", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "TwitchUser")
                        .WithMany("GiftedSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchUser");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.Subscription", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "TwitchUser")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchUser");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.SupportTotal", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "TwitchUser")
                        .WithOne("SupportTotal")
                        .HasForeignKey("Koishibot.Core.Features.Supports.Models.SupportTotal", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchUser");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Supports.Models.TwitchCheer", b =>
                {
                    b.HasOne("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", "TwitchUser")
                        .WithMany("Cheers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchUser");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChannelPoints.Models.ChannelPointReward", b =>
                {
                    b.Navigation("ChannelPointRedemptions");
                });

            modelBuilder.Entity("Koishibot.Core.Features.ChatCommands.Models.ChatCommand", b =>
                {
                    b.Navigation("CommandNames");
                });

            modelBuilder.Entity("Koishibot.Core.Features.Dandle.Models.DandleWord", b =>
                {
                    b.Navigation("DandleResults");
                });

            modelBuilder.Entity("Koishibot.Core.Features.StreamInformation.Models.TwitchStream", b =>
                {
                    b.Navigation("DandleResult")
                        .IsRequired();

                    b.Navigation("IncomingRaids");

                    b.Navigation("OutgoingRaid");

                    b.Navigation("PollResults");
                });

            modelBuilder.Entity("Koishibot.Core.Features.StreamInformation.Models.YearlyQuarter", b =>
                {
                    b.Navigation("TwitchStreams");
                });

            modelBuilder.Entity("Koishibot.Core.Features.TwitchUsers.Models.TwitchUser", b =>
                {
                    b.Navigation("Attendance");

                    b.Navigation("ChannelFollows");

                    b.Navigation("Cheers");

                    b.Navigation("GiftedSubscriptions");

                    b.Navigation("RaidsFromThisUser");

                    b.Navigation("RaidsSuggestedByThisUser");

                    b.Navigation("RedeemedChannelPointRewards");

                    b.Navigation("Subscriptions");

                    b.Navigation("SupportTotal");

                    b.Navigation("UsersSuggestingThisRaidTarget");
                });
#pragma warning restore 612, 618
        }
    }
}
