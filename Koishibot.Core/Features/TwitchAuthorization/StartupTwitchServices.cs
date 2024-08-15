using Koishibot.Core.Features.Obs;
using Koishibot.Core.Features.StreamInformation;
using Koishibot.Core.Services.StreamElements;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.TwitchAuthorization;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record StartupTwitchServices(
ITwitchEventSubService TwitchEventSubService,
ITwitchIrcService TwitchIrcService,
IValidateTokenService ValidateTokenService
) : IStartupTwitchServices
{
	public async Task Start()
	{
		await ValidateTokenService.Start();
		await TwitchIrcService.CreateWebSocket();
		await TwitchEventSubService.CreateWebSocket();
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IStartupTwitchServices
{
	Task Start();
}