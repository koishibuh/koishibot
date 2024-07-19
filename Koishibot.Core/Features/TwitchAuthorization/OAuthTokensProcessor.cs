using Koishibot.Core.Features.Application;
using System.Net;
namespace Koishibot.Core.Features.TwitchAuthorization;

public record OAuthTokensProcessor(
  ILogger<OAuthTokensProcessor> Log,
  IOptions<Settings> Settings, ITwitchAPI TwitchApi,
	IServiceScopeFactory ScopeFactory
	) : IOAuthTokensProcessor
{
  public TwitchAppSettings TwitchApp => Settings.Value.TwitchAppSettings;
	private HttpListener _listener = new HttpListener();

	public async void StartListener()
  {
    _listener.Prefixes.Add(Settings.Value.TwitchAppSettings.RedirectUri);

    _listener.Start();
    Log.LogInformation("Listener for Oauth Code Started");

		var timer = new Timer(state => CloseListener(), 
			null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));

		while (_listener.IsListening)
		{
			var context = await _listener.GetContextAsync();
			var request = context.Request;

			var code = request.QueryString["code"];

			_listener.Stop();
			Log.LogInformation("Listener Closed");

			await StoreAccessToken(code);
			await StoreStreamerInfo();

			// Publish to UI that Connection is valid

			using var scope = ScopeFactory.CreateScope();
			var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

			await mediatr.Send(new StartupTwitchServicesCommand());
		}
	}

	// == ⚫ == //

	private void CloseListener()
	{
		if (_listener is not null && _listener.IsListening)
		{
			_listener.Stop();
			Console.WriteLine("HttpListener stopped due to timeout");
		}
	}

	public async Task StoreAccessToken(string? code)
   {
    if (code is null)
    {
        // publish oauth is invalid
        return;
    }

    var result = await TwitchApi.Auth.GetAccessTokenFromCodeAsync
        (code, TwitchApp.ClientSecret, TwitchApp.RedirectUri);

    TwitchApi.Settings.AccessToken = result.AccessToken;
    Settings.Value.StreamerTokens.SetExpirationTime(result.ExpiresIn);
    Settings.Value.StreamerTokens.AccessToken = result.AccessToken;
    Settings.Value.StreamerTokens.RefreshToken = result.RefreshToken;
	}

  public async Task StoreStreamerInfo()
  {
    var result = await TwitchApi.Helix.Users.GetUsersAsync();
    var user = result.Users[0];

    Settings.Value.StreamerTokens.Username = user.Login;
    Settings.Value.StreamerTokens.UserId = user.Id;
  }
}
