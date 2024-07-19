using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Koishibot.Core.Services.Google;
public class GoogleService : IGoogleService
{
	public GoogleTokens settings { get; set; }

	public CalendarService GoogleCalendar { get; set; }

	public GoogleService(IOptions<Settings> Settings)
	{
		settings = Settings.Value.GoogleTokens;
		GoogleCalendar = GetGoogleCalendar();
	}

	public CalendarService GetGoogleCalendar()
	{
		if (GoogleCalendar is null)
		{
			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(settings.ClientEmail)
			{
				Scopes = new[] { CalendarService.Scope.Calendar }
			}.FromPrivateKey(settings.PrivateKey));

			return new CalendarService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = settings.ApplicationName
			});
		}
		else
		{
			return GoogleCalendar;
		}
	}

	public void GetEvents(DateTime date, string calendarId)
	{

	}

	public void CreateEvent()
	{

	}

}

public interface IGoogleService
{
	CalendarService GetGoogleCalendar();

}