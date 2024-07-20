using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.StreamInformation.ViewModels;
using Microsoft.AspNetCore.SignalR;
namespace Koishibot.Core.Services.SignalR;

// == ⚫ GENERAL == //

public partial record SignalrService(
	IHubContext<SignalrHub, ISignalrHub> HubContext
	): ISignalrService
{
	public async Task SendLog(LogVm log) =>
		await HubContext.Clients.All.ReceiveLog(log);

	public async Task SendNotification(string content) => 
		await HubContext.Clients.All.ReceiveNotification(content);

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

	public async Task SendPoll(PollVm pollVm) =>
		await HubContext.Clients.All.ReceivePoll(pollVm);

	public async Task SendAdStartedEvent(AdBreakVm adBreakVm) =>
		await HubContext.Clients.All.ReceiveAdStartedEvent(adBreakVm);

}


// == ⚫ SEND INTERFACE == //
public partial interface ISignalrService
{
	Task SendLog(LogVm log);
	Task SendNotification(string content);
	Task SendChatMessage(ChatMessageVm chatMessageVm);
	Task SendStatusUpdate(ServiceStatusVm serviceStatusVM);
	Task SendStreamEvent(StreamEventVm streamEventVm);
	Task SendStreamInfo(StreamInfoVm streamInfoVm);
	Task SendOverlayStatus(OverlayStatusVm overlayStatusVm);
	Task SendOverlayTimer(OverlayTimerVm timer);
	Task SendPromoVideoUrl(string url);
	Task SendPoll(PollVm pollVm);
	Task SendAdStartedEvent(AdBreakVm adBreakVm);
}


// == ⚫ RECEIVE INTERFACE == //

public partial interface ISignalrHub
{
	Task ReceiveLog(LogVm log);
	Task ReceiveNotification(string content);
	Task ReceiveChatMessage(ChatMessageVm chatMessageVm);
	Task ReceiveStatusUpdate(ServiceStatusVm serviceStatusVM);
	Task ReceiveStreamEvent(StreamEventVm streamEventVm);
	Task ReceiveStreamInfo(StreamInfoVm streamInfoVm);
	Task ReceiveOverlayStatus(OverlayStatusVm overlayStatusVm);
	Task ReceiveOverlayTimer(OverlayTimerVm overlayTimerVm);
	Task ReceivePromoVideoUrl(string url);
	Task ReceivePoll(PollVm pollVm);
	Task ReceiveAdStartedEvent(AdBreakVm adBreakVm);
}