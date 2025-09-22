using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.Irc;
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

		await Execute(() => TwitchIrcService.CreateWebSocket());
		await Execute(() => TwitchEventSubService.CreateWebSocket());
	}

	private static async Task Execute(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IStartupTwitchServices
{
	Task Start();
}