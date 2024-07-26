

//using Koishibot.Core.Features.Polls.Models;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Persistence.Cache.Enums;

//namespace Koishibot.Core.Features.Polls.Extensions;
//public static class PollExtensions
//{
//	public static void AddPoll(this IAppCache cache, CurrentPoll poll)
//	{
//		cache.Add(CacheName.CurrentPoll, poll);
//	}

//	public static async Task AddPollResult(this KoishibotDbContext database,
//		PollResult pollResult)
//	{
//		database.Update(pollResult);
//		await database.SaveChangesAsync();
//	}
//}
