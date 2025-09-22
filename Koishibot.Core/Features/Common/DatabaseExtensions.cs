using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.Common;

public static class DatabaseExtensions
{
	public static async Task<ChatResponseDto?> GetResponse
		(this KoishibotDbContext database, string commandName) =>
		await database.ChatResponses
			.Where(x => x.InternalName == commandName)
			.Select(x => new ChatResponseDto(x.Id, x.InternalName, x.Description, x.Message))
			.FirstOrDefaultAsync();
	
	public static async Task<LiveStream?> GetLastStream
		(this KoishibotDbContext database)
	{
		var result = await database.LiveStreams
			.OrderByDescending(s => s.Id)
			.FirstOrDefaultAsync();

		return result;
	}

	public static async Task<int?> GetLastMandatorySessionId(this KoishibotDbContext database, int streamSessionId)
	{
		var result = await database.StreamSessions
			.OrderByDescending(x => x.Id)
			.Where(x => x.AttendanceMandatory == true && x.Id != streamSessionId)
			.FirstOrDefaultAsync();

		return result.Id;
	}
	
	public static async Task<int?> GetLastMandatorySessionId(this KoishibotDbContext database)
	{
		var result = await database.StreamSessions
			.OrderByDescending(x => x.Id)
			.Where(x => x.AttendanceMandatory == true)
			.FirstOrDefaultAsync();

		return result.Id;
	}


	// public static async Task<DateOnly?> GetLastMandatoryStreamDate
	// 		(this KoishibotDbContext database)
	// {
	// 	var result = await database.TwitchStreams
	// 			.OrderByDescending(s => s.Id)
	// 			.Skip(1)
	// 			.FirstOrDefaultAsync(s => s.AttendanceMandatory == true);
	//
	// 	return result is null
	// 	? null
	// 	: DateOnly.FromDateTime(result.StartedAt.DateTime);
	// }

	public static async Task<YearlyQuarter?> GetYearlyQuarter
		(this KoishibotDbContext database)
	{
		return await database.YearlyQuarters
			.OrderByDescending(o => o.Id)
			.FirstOrDefaultAsync();
	}

	public static async Task<bool> WasRewardRedeemedToday
		(this KoishibotDbContext database, int rewardId)
	{
		var result = await database.ChannelPointRedemptions
			.Where(p => p.ChannelPointRewardId == rewardId
				&& p.Timestamp.Date == DateTime.UtcNow.Date
				&& p.WasSuccesful == true)
			.FirstOrDefaultAsync();

		return result is not null;
	}
}