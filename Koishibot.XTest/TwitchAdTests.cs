using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace Koishibot.XTest;

public class TwitchAdTests : IClassFixture<KoishibotDbContext>
{
	public class TwitchAdContextFactory : IDisposable
	{
		private readonly DbContextOptions<KoishibotDbContext> _options;
		public KoishibotDbContext context;

		public TwitchAdContextFactory()
		{
			_options = new DbContextOptionsBuilder<KoishibotDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new KoishibotDbContext(_options);
		}

		[Fact]
		public async void ConvertDto()
		{
			// Arrange
			var text = File.ReadAllText("../../../TwitchApiData/GetAdScheduleData.jsonl");
			var data = JsonSerializer.Deserialize<GetAdScheduleResponse>(text);

			// Act
			var dto = data.Data[0].ConvertToDto();
			var time = dto.CalculateAdjustedTimeUntilNextAd();

			// Assert
			Assert.NotNull(dto);
		}




		public void Dispose()
		{
			context.Dispose();
		}
	}
}