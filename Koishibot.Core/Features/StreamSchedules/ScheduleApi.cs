namespace Koishibot.Core.Features.StreamSchedules;

//public record ScheduleApi(ITwitchAPI TwitchApi,
//    IOptions<Settings> Settings,
  
//    ILogger<ScheduleApi> Log)
//{
//    public string StreamerId => Settings.Value.StreamerTokens.UserId;

//    /// <summary>
//    /// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-stream-schedule">Get Stream Schedule Documentation</see>
//    /// </summary>
//    /// <returns></returns>
//    //public async Task GetStreamSchedule()
//    //{
//    //	await TokenProcessor.EnsureValidToken();

//    //	var streamerId = Settings.Value.StreamerTokens.UserId;

//    //	var result = await TwitchApi.Helix.Schedule.GetChannelStreamScheduleAsync(streamerId);
//    //	if (result is null || result.Schedule.Segments.Length == 0)
//    //	{
//    //		return null;
//    //	}

//    //	//result.Schedule.Segments[0].Id
//    //	//result.Schedule.Segments[0].StartTime
//    //	//result.Schedule.Segments[0].EndTime
//    //	//result.Schedule.Segments[0].Title
//    //	//result.Schedule.Segments[0].CanceledUntil
//    //	//result.Schedule.Segments[0].Category.Id
//    //	//result.Schedule.Segments[0].Category.Name
//    //	//result.Schedule.Segments[0].IsRecurring
//    //	//result.Schedule.Vacation.StartTime
//    //	//result.Schedule.Vacation.EndTime

//    //}



//    /// <summary>
//    /// <see href="https://dev.twitch.tv/docs/api/reference/#update-channel-stream-schedule">Update Stream Schedule Documentation</see>
//    /// </summary>
//    /// <returns></returns>
//    //public async Task UpdateStreamSchedule()
//    //{
//    //	//await TwitchApi.Helix.Schedule.UpdateChannelStreamScheduleAsync();

//    //}
//}
