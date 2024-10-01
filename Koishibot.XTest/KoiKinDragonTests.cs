using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace Koishibot.XTest;

public class KoiKinDragonTests : IClassFixture<KoishibotDbContext>
{
	public class KoiKinDragonContextFactory : IDisposable
	{
		private readonly DbContextOptions<KoishibotDbContext> _options;
		public KoishibotDbContext context;

		public KoiKinDragonContextFactory()
		{
			_options = new DbContextOptionsBuilder<KoishibotDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new KoishibotDbContext(_options);
		}

		[Fact]
		public async void GetLastUnnamedDragon_ReturnsDragon()
		{
			//Arrange

			var user = new TwitchUser
			{
				Id = 1,
				TwitchId = "98683749",
				Name = "ElysiaGriffin"
			};
			context.Users.Add(user);


			var tag = new WordpressItemTag(user.Id, 001);
			context.WordpressItemTags.Add(tag);


			var dragons = new List<KoiKinDragon>
			{
				new()
				{
					Code = "NamedDragon1",
					Name = "Name",
					Timestamp = DateTimeOffset.UtcNow.AddDays(-31),
					WordpressId = 1,
					ItemTagId = 1
				},
				new()
				{
					Code = "UnnamedDragon3",
					Name = "?",
					Timestamp = DateTimeOffset.UtcNow.AddDays(-14),
					WordpressId = 3,
					ItemTagId = 1
				},
				new()
				{
					Code = "UnnamedDragon2",
					Name = "?",
					Timestamp = DateTimeOffset.UtcNow.AddDays(-15),
					WordpressId = 2,
					ItemTagId = 1
				},
				new()
				{
					Code = "UnnamedDragon4",
					Name = "?",
					Timestamp = DateTimeOffset.UtcNow.AddDays(-13),
					WordpressId = 4,
					ItemTagId = 1
				}
			};


			context.KoiKinDragons.AddRange(dragons);
			context.SaveChanges();

			// Act
			var result = await context.GetLastUnnamedAdultDragon();

			// Assert
			Assert.True(result.code == "UnnamedDragon2");
			Assert.True(result.username == user.Name);
		}



		public void Dispose() { }
	}
}