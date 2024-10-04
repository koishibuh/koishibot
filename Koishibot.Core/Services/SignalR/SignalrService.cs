using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.StreamInformation.ViewModels;
using Microsoft.AspNetCore.SignalR;
namespace Koishibot.Core.Services.SignalR;

/*═══════════════════【 GENERAL 】═══════════════════*/
public partial record SignalrService(
IHubContext<SignalrHub,
ISignalrHub> HubContext,
ILogger<SignalrService> Log
) : ISignalrService
{
	public async Task SendInfo(string message)
	{
		Log.LogInformation(message);
		var log = new LogVm(message, "Info", Toolbox.CreateUITimestamp());
		await HubContext.Clients.All.ReceiveInfo(log);
	}

	public async Task SendError(string message)
	{
		Log.LogError(message);
		var log = new LogVm(message, "Error", Toolbox.CreateUITimestamp());
		await HubContext.Clients.All.ReceiveError(log);
	}

	public async Task SendLog(string message)
	{
		Log.LogInformation(message);
		var log = new LogVm(message, "Info", Toolbox.CreateUITimestamp());
		await HubContext.Clients.All.ReceiveLog(log);
	}

	public async Task SendNewNotification(NotificationVm notificationVm) =>
		await HubContext.Clients.All.ReceiveNewNotification(notificationVm);

	public async Task SendChatMessage(ChatMessageVm chatMessageVM) =>
		await HubContext.Clients.All.ReceiveChatMessage(chatMessageVM);

	public async Task SendStatusUpdate(ServiceStatusVm serviceStatusVM) =>
		await HubContext.Clients.All.ReceiveStatusUpdate(serviceStatusVM);

	public async Task SendStreamEvent(StreamEventVm streamEventVm) =>
		await HubContext.Clients.All.ReceiveStreamEvent(streamEventVm);

	public async Task SendStreamInfo(StreamInfoVm streamInfoVM) =>
		await HubContext.Clients.All.ReceiveStreamInfo(streamInfoVM);

	public async Task SendOverlayStatus(OverlayStatusVm overlayStatusVm) =>
		await HubContext.Clients.All.ReceiveOverlayStatus(overlayStatusVm);

	public async Task SendOverlayTimer(OverlayTimerVm overlayTimerVm) =>
		await HubContext.Clients.All.ReceiveOverlayTimer(overlayTimerVm);

	public async Task SendPromoVideoUrl(string url) =>
		await HubContext.Clients.All.ReceivePromoVideoUrl(url);

	public async Task SendPollStarted(PollVm pollVm) =>
		await HubContext.Clients.All.ReceivePollStarted(pollVm);

	public async Task SendPollCancelled() =>
		await HubContext.Clients.All.ReceivePollCancelled();

	public async Task SendPollVote(List<PollChoiceInfo> pollVotesVm) =>
		await HubContext.Clients.All.ReceivePollVote(pollVotesVm);

	public async Task SendPollEnded(string winner) =>
		await HubContext.Clients.All.ReceivePollEnded(winner);

	public async Task SendAdStartedEvent(AdBreakVm adBreakVm) =>
		await HubContext.Clients.All.ReceiveAdStartedEvent(adBreakVm);
}

/*═══════════════【 SEND INTERFACE 】═══════════════*/
public partial interface ISignalrService
{
	Task SendInfo(string message);
	Task SendError(string message);
	Task SendLog(string message);
	Task SendNewNotification(NotificationVm notificationVm);
	Task SendChatMessage(ChatMessageVm chatMessageVm);
	Task SendStatusUpdate(ServiceStatusVm serviceStatusVM);
	Task SendStreamEvent(StreamEventVm streamEventVm);
	Task SendStreamInfo(StreamInfoVm streamInfoVm);
	Task SendOverlayStatus(OverlayStatusVm overlayStatusVm);
	Task SendOverlayTimer(OverlayTimerVm timer);
	Task SendPromoVideoUrl(string url);
	Task SendPollStarted(PollVm pollVm);
	Task SendPollCancelled();
	Task SendPollVote(List<PollChoiceInfo> pollChoiceInfo);
	Task SendPollEnded(string winner);
	Task SendAdStartedEvent(AdBreakVm adBreakVm);
}

/*═════════════【 RECEIVE INTERFACE 】═════════════*/
public partial interface ISignalrHub
{
	Task ReceiveLog(LogVm log);
	Task ReceiveInfo(LogVm log);
	Task ReceiveError(LogVm log);
	Task ReceiveNewNotification(NotificationVm notificationVm);
	Task ReceiveChatMessage(ChatMessageVm chatMessageVm);
	Task ReceiveStatusUpdate(ServiceStatusVm serviceStatusVM);
	Task ReceiveStreamEvent(StreamEventVm streamEventVm);
	Task ReceiveStreamInfo(StreamInfoVm streamInfoVm);
	Task ReceiveOverlayStatus(OverlayStatusVm overlayStatusVm);
	Task ReceiveOverlayTimer(OverlayTimerVm overlayTimerVm);
	Task ReceivePromoVideoUrl(string url);
	Task ReceivePollStarted(PollVm pollVm);
	Task ReceivePollCancelled();
	Task ReceivePollVote(List<PollChoiceInfo> pollChoiceInfo);
	Task ReceivePollEnded(string winner);
	Task ReceiveAdStartedEvent(AdBreakVm adBreakVm);
}