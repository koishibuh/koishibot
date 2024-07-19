using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Application.Controllers;

// == ⚫ GET == //

public class GetEventFeedController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Event Feed"])]
	[HttpGet("/api/event-feed")]
	public async Task<ActionResult> GetEventFeed()
	{
		var result = await Mediator.Send(new GetEventFeedCommand());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record GetEventFeedCommand()
	: IRequest<List<StreamEventVm>>;

// == ⚫ HANDLER == //

public record GetEventFeedHandler(
	KoishibotDbContext Database
) : IRequestHandler<GetEventFeedCommand, List<StreamEventVm>>
{
	public async Task<List<StreamEventVm>> Handle
		(GetEventFeedCommand command, CancellationToken cancel)
	{
		// get follow & raid
		return await Database.GetRecentStreamEvents();
	}
}

// == ⚫ EXTENSIONS == //

public static class GetEventFeedExtensions
{
	public static async Task<List<StreamEventVm>> GetRecentStreamEvents
		(this KoishibotDbContext database)
	{
		var follows = await database.ChannelFollows
			.AsNoTracking()
			.Include(x => x.TwitchUser)
			.OrderByDescending(f => f.Timestamp)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Follow,
				Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.TwitchUser.Name} has followed"
			})
			.ToListAsync();

		var raids = await database.IncomingRaids
			.AsNoTracking()
			.Include(x => x.RaidedByUser)
			.OrderByDescending(f => f.RaidedAt)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Raid,
				Timestamp = f.RaidedAt.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.RaidedByUser.Name} has raided with {f.ViewerCount}"
			})
			.ToListAsync();

		var list = new List<StreamEventVm>();
		list.AddRange(follows);
		list.AddRange(raids);
		var updatedList = list.OrderByDescending(x => x.Timestamp).ToList();
		return updatedList;
		}
}