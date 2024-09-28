using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

namespace Koishibot.XTest;

public class ChannelPointRewardTests : IClassFixture<KoishibotDbContext>
{
	public class ChannelPointRewardContextFactory : IDisposable
	{
		private readonly DbContextOptions<KoishibotDbContext> _options;
		public KoishibotDbContext context;

		public ChannelPointRewardContextFactory()
		{
			_options = new DbContextOptionsBuilder<KoishibotDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			var channelPoint = new ChannelPointReward
			{
				CreatedOn = DateTimeOffset.Now,
				TwitchId = "cbe70565-0250-4221-9f68-a3436d4b382f",
				Title = "Dragon Egg Quest",
				Description = "Try your luck with acquiring a dragon egg",
				Cost = 200,
				BackgroundColor = "#53B8EA",
				IsEnabled = true,
				IsUserInputRequired = false,
				IsMaxPerStreamEnabled = false,
				MaxPerStream = 1,
				IsMaxPerUserPerStreamEnabled = false,
				MaxPerUserPerStream = 0,
				IsGlobalCooldownEnabled = true,
				GlobalCooldownSeconds = 60,
				IsPaused = false,
				ShouldRedemptionsSkipRequestQueue = true,
				ImageUrl = "https://static-cdn.jtvnw.net/custom-reward-images/default-4.png"
			};

			context = new KoishibotDbContext(_options);

			context.ChannelPointRewards.AddRange(channelPoint);
			context.SaveChanges();
		}


		[Fact]
		public void GetChannelRewardByName_ReturnsReward()
		{
			// Arrange

			// Act
			var result = context.GetChannelRewardByName("Dragon Egg Quest");

			// Assert
			Assert.Equal(result.Result.TwitchId, "cbe70565-0250-4221-9f68-a3436d4b382f");
		}

		[Fact]
		public void GetChannelRewards_ReturnsReward()
		{
			// Arrange

			// Act
			var result = context.GetChannelPointRewards();

			// Assert
			Assert.True(result.Result.Count == 1);
		}

		[Fact]
		public async void GetTodayRedemptionByRewardId_ReturnsZero()
		{
			// Arrange

			// Act
			var result = await context.GetTodayRedemptionByRewardId(1);

			// Assert
			Assert.True(result.NoRedemptionsToday());
		}

		[Fact]
		public async void GetTodayRedemptionByRewardId_ReturnsTwoCount()
		{
			var user = new TwitchUser
			{
				Id = 1,
				TwitchId = "98683749",
				Name = "ElysiaGriffin"
			};

			// Arrange
			var redemptions = new List<ChannelPointRedemption>
			{
				new()
				{
					ChannelPointRewardId = 1,
					RedeemedAt = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(-5),
					UserId = 1,
					WasSuccesful = false
				},
				new()
				{
					ChannelPointRewardId = 1,
					RedeemedAt = DateTimeOffset.UtcNow,
					UserId = 1,
					WasSuccesful = false
				}
			};

			context.Users.AddRange(user);
			context.ChannelPointRedemptions.AddRange(redemptions);
			context.SaveChanges();

			// Act
			var result = await context.GetTodayRedemptionByRewardId(1);

			// Assert
			Assert.True(result.Count == 2);
			Assert.False(result.NoRedemptionsToday());
		}

		public void Dispose()
		{
		}
	}
}