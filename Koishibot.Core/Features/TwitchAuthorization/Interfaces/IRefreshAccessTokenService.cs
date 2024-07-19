namespace Koishibot.Core.Features.TwitchAuthorization.Interfaces;
public interface IRefreshAccessTokenService
{
	Task<bool> Start();
	void SaveSettingsToJson();
	Task EnsureValidToken();
	Task EnsureValidOnStartup();
}