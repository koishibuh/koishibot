using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Koishibot.Core.Services.Google;

namespace Koishibot.Core.Features.StreamSchedules;
public record GoogleCalendarApi(IGoogleService GoogleService) : IGoogleCalendarApi
{
	public async Task<IList<Event>> GetEvents(string calendarId)
	{
		var request = GoogleService.GetGoogleCalendar().Events.List(calendarId);
		request.TimeMaxDateTimeOffset = DateTime.Now;
		request.ShowDeleted = false;
		request.SingleEvents = true;
		request.MaxResults = 10;
		request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

		Events events = await request.ExecuteAsync();
		return events.Items;
	}
}

public interface IGoogleCalendarApi
{
	Task<IList<Event>> GetEvents(string calendarId);
}