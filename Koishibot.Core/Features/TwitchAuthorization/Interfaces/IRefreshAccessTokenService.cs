namespace Koishibot.Core.Features.TwitchAuthorization.Interfaces;
public interface IRefreshAccessTokenService
{
	Task<bool> Start();
	Task<bool> StartWithToken(string token);
	void SaveSettingsToJson();
	Task EnsureValidToken();
	Task EnsureValidOnStartup();
}