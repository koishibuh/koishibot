using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.StreamElements.Models;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Lol what documentation</para>
/// </summary>
public record SETipReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database,
	ITwitchApiRequest TwitchApiRequest
	) : IRequestHandler<SETipReceivedCommand>
{
	public async Task Handle(SETipReceivedCommand command, CancellationToken cancel)
	{
		SETip seTip = new();

		if (command.ProviderIsTwitch())
		{
			var userDto = command.CreateUserDto();
			var user = await TwitchUserHub.Start(userDto);

			seTip = command.CreateTip(user);
		}
		else
		{
			seTip = command.CreateTip(null);
		}

		await Database.UpdateEntry(seTip);
		
		// TODO: Combine tip total
		//await Database.UpdateSubDurationTotal(sub, command.e.CumulativeMonths);

		var seTipVm = command.CreateVm();
		await Signalr.SendStreamEvent(seTipVm);
	}
}

// == ⚫ COMMAND == //

public record SETipReceivedCommand(
	StreamElementsEvent e
	) : IRequest
{
	public bool ProviderIsTwitch()
	{
		return e.Provider == "twitch";
	}

	public TwitchUserDto CreateUserDto()
	{
		return new TwitchUserDto(
			e.Data.UserId,
			e.Data.UserLogin,
			e.Data.UserName);
	}

	public SETip CreateTip(TwitchUser? user)
	{
		return new SETip
		{
			StreamElementsId = e.Id,
			Timestamp = e.CreatedAt,
			UserId = user is not null ? user.Id : null,
			Username = e.Data.UserName,
			Message = e.Data.Message ?? string.Empty,
			Amount = e.Data.Amount.ToString()
		};
	}

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.SETip,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{e.Data.UserName} has tipped {e.Data.Amount} through StreamElements"
		};
	}
};