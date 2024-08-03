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
		// get follow, raid, cheers, subs
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
			.OrderByDescending(f => f.Timestamp)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Raid,
				Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.RaidedByUser.Name} has raided with {f.ViewerCount}"
			})
			.ToListAsync();

		var cheers = await database.Cheers
			.AsNoTracking()
			.Include(x => x.TwitchUser)
			.OrderByDescending(f => f.Timestamp)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Cheer,
				Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.TwitchUser.Name} has cheered {f.BitsAmount}"
			})
			.ToListAsync();

		var subs = await database.Subscriptions
			.AsNoTracking()
			.Include(x => x.TwitchUser)
			.OrderByDescending(f => f.Timestamp)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Sub,
				Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.TwitchUser.Name} has subscribed at {f.Tier} for 1 month"
			})
			.ToListAsync();

		var giftsub = await database.GiftSubscriptions
			.AsNoTracking()
			.Include(x => x.TwitchUser)
			.OrderByDescending(f => f.Timestamp)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Sub,
				Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.TwitchUser.Name} has gifted {f.Total} {f.Tier} subs!"
			})
			.ToListAsync();

		var list = new List<StreamEventVm>();
		list.AddRange(follows);
		list.AddRange(raids);
		list.AddRange(cheers);
		list.AddRange(subs);
		list.AddRange(giftsub);
		var updatedList = list.OrderByDescending(x => x.Timestamp).ToList();
		return updatedList;
	}
}