//using Koishibot.Core.Features.Application;
//using System.Net;
//namespace Koishibot.Core.Features.TwitchAuthorization;

//public record OAuthTokensProcessor(
//	ILogger<OAuthTokensProcessor> Log,
//	IOptions<Settings> Settings, ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//	) : IOAuthTokensProcessor
//{
//	public TwitchAppSettings TwitchApp => Settings.Value.TwitchAppSettings;

//	public async void StartListener()
//	{
//		var listener = new HttpListener();

//		listener.Prefixes.Add(Settings.Value.TwitchAppSettings.RedirectUri);

//		listener.Start();
//		Log.LogInformation("Listener for Oauth Code Started");

//		var timer = new Timer(state => CloseListener(listener),
//			null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));

//		while (listener.IsListening)
//		{
//			Log.LogInformation("Listener is listening");
//			var context = await listener.GetContextAsync();
//			Log.LogInformation("Context created");
//			var request = context.Request;

//			var code = request.QueryString["code"];

//			context.Response.Redirect("https://koishibot.elysiagriffin.com/bot/");

//			listener.Close();
//			Log.LogInformation("Listener Closed");

//			await StoreAccessToken(code);
//			await StoreStreamerInfo();

//			// Publish to UI that Connection is valid

//			using var scope = ScopeFactory.CreateScope();
//			var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//			await mediatr.Send(new StartupTwitchServicesCommand());
//		}
//	}

//	// == ⚫ == //

//	private void CloseListener(HttpListener listener)
//	{
//		if (listener is not null && listener.IsListening)
//		{
//			listener.Close();
//			Console.WriteLine("HttpListener stopped due to timeout");
//		}
//	}

//	public async Task StoreAccessToken(string? code)
//	{
//		if (code is null)
//		{
//			// publish oauth is invalid
//			return;
//		}

//		var result = await TwitchApi.Auth.GetAccessTokenFromCodeAsync
//				(code, TwitchApp.ClientSecret, TwitchApp.RedirectUri);

//		TwitchApi.Settings.AccessToken = result.AccessToken;
//		Settings.Value.StreamerTokens.SetExpirationTime(result.ExpiresIn);
//		Settings.Value.StreamerTokens.AccessToken = result.AccessToken;
//		Settings.Value.StreamerTokens.RefreshToken = result.RefreshToken;
//	}

//	public async Task StoreStreamerInfo()
//	{
//		var result = await TwitchApi.Helix.Users.GetUsersAsync();
//		var user = result.Users[0];

//		Settings.Value.StreamerTokens.Username = user.Login;
//		Settings.Value.StreamerTokens.UserId = user.Id;
//	}
//}
