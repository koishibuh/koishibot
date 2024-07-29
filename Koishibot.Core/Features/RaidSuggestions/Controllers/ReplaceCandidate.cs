using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Models;
namespace Koishibot.Core.Features.RaidSuggestions.Controllers;

// == ⚫ PATCH == //

public class ReplaceCandidateController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Outgoing Raid"])]
	[HttpPatch("/api/raid/suggestions/")]
	public async Task<ActionResult> ReplaceCandidate
		([FromBody] ReplaceCandidateCommand command)
	{
		var result = await Mediator.Send(command);
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record ReplaceCandidateCommand(
	string StreamerName)
	: IRequest<RaidCandidateVm>;

// == ⚫ HANDLER == //

public record ReplaceCandidateHandler(
	IAppCache Cache
	) : IRequestHandler<ReplaceCandidateCommand, RaidCandidateVm>
{
	public async Task<RaidCandidateVm> Handle
		(ReplaceCandidateCommand command, CancellationToken cancel)
	{
		var result = Cache.GetRaidSuggestions();
		var streamer = result.SelectARandomCandidate()
			?? throw new Exception("Unable to find another raid candidate");

		var candidates = Cache.GetRaidCandidates()
			?? throw new Exception("Raid Candidates list not found");

		var removingStreamer = candidates.Find(x => x.Streamer.Name == command.StreamerName)
			?? throw new Exception("Can't find streamer in Raid Candidates list");

		candidates.Remove(removingStreamer);
		candidates.Add(streamer);

		Cache.AddRaidCandidates(candidates);

		return await Task.FromResult(candidates.ConvertToVm());
	}
}