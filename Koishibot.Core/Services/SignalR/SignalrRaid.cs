using Koishibot.Core.Features.RaidSuggestions.Models;
namespace Koishibot.Core.Services.SignalR;

// == ⚫ RAID == //

public partial record SignalrService : ISignalrService
{
	public async Task SendRaidOverlayStatus(bool overlayStatus) =>
	await HubContext.Clients.All.ReceiveRaidOverlayStatus(overlayStatus);

	public async Task SendRaidCandidates(RaidCandidateVm raidCandidatesVm) =>
		await HubContext.Clients.All.ReceiveRaidCandidates(raidCandidatesVm);

	public async Task SendRaidPollVote(PollVotesVm pollVotesVm) =>
		await HubContext.Clients.All.ReceiveRaidPollVote(pollVotesVm);
}

// == ⚫ SEND INTERFACE == //

public partial interface ISignalrService
{
	Task SendRaidOverlayStatus(bool overlayStatus);
	Task SendRaidCandidates(RaidCandidateVm raidCandidatesVm);
	Task SendRaidPollVote(PollVotesVm pollVotesVm);
}

// == ⚫ RECEIVE INTERFACE == //

public partial interface ISignalrHub
{
	Task ReceiveRaidOverlayStatus(bool overlayStatus);
	Task ReceiveRaidCandidates(RaidCandidateVm raidCandidatesVm);
	Task ReceiveRaidPollVote(PollVotesVm pollVotesVm);
}