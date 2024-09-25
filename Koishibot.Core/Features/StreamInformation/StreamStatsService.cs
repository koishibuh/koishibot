using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.StreamInformation;

/*═══════════════════【 SERVICE 】═══════════════════*/
public class StreamStatsService(
ITwitchApiRequest twitchApiRequest,
IServiceScopeFactory scopeFactory
) : IStreamStatsService
{
	public async Task Start()
	{
		using var scope = scopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		var parameters = new GetLiveStreamsRequestParameters { UserLogins = ["elysiagriffin"] };

		var result = await twitchApiRequest.GetLiveStreams(parameters);
		var streamInfo = result.Data[0];

		var streamSession = await database.GetRecentStreamSession();

		var categoryId = await database.GetStreamCategoryId(streamInfo.CategoryId, streamInfo.CategoryName);

		var stats = new StreamStats
		{
			StreamSessionId = streamSession.Id,
			StreamTitle = streamInfo.StreamTitle,
			StreamCategoryId = categoryId,
			ViewerCount = streamInfo.ViewerCount
		};

		await database.UpdateEntry(stats);
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IStreamStatsService
{
	Task Start();
}