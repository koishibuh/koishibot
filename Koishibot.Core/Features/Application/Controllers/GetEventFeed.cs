using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Application.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/event-feed")]
public class GetEventFeedController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Event Feed"])]
	[HttpGet]
	public async Task<ActionResult> GetEventFeed()
	{
		var result = await Mediator.Send(new GetEventFeedCommand());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
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

/*════════════════════【 QUERY 】════════════════════*/
public record GetEventFeedCommand : IRequest<List<StreamEventVm>>;

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
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
				Message = $"{f.TwitchUser.Name} has subscribed"
			})
			.ToListAsync();
		
		// TODO: adjust this based on type
		var kofi = await database.Kofis
			.AsNoTracking()
			.OrderByDescending(f => f.Timestamp)
			.Take(50)
			.Select(f => new StreamEventVm
			{
				EventType = StreamEventType.Sub,
				Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{f.Username} tipped through Kofi for {f.AmountInPence}"
			})
			.ToListAsync();

		// var giftsub = await database.GiftSubscriptions
		// 	.AsNoTracking()
		// 	.Include(x => x.TwitchUser)
		// 	.OrderByDescending(f => f.Timestamp)
		// 	.Take(50)
		// 	.Select(f => new StreamEventVm
		// 	{
		// 		EventType = StreamEventType.Sub,
		// 		Timestamp = f.Timestamp.ToString("yyyy-MM-dd HH:mm"),
		// 		Message = $"{f.TwitchUser.Name} has gifted {f.Total} {f.Tier} subs!"
		// 	})
		// 	.ToListAsync();

		var pointRedemption = await database.ChannelPointRedemptions
			.AsNoTracking()
			.Include(x => x.TwitchUser)
			.OrderByDescending(x => x.Timestamp)
			.Take(50)
			.Select(x => new StreamEventVm
			{
				EventType = StreamEventType.ChannelPoint,
				Timestamp = x.Timestamp.ToString("yyyy-MM-dd HH:mm"),
				Message = $"{x.TwitchUser.Name} has redeemed {x.ChannelPointReward.Title}"
			})
			.ToListAsync();

		var list = new List<StreamEventVm>();
		list.AddRange(follows);
		list.AddRange(raids);
		list.AddRange(cheers);
		list.AddRange(subs);
		list.AddRange(kofi);
		// list.AddRange(giftsub);
		list.AddRange(pointRedemption);
		var updatedList = list.OrderBy(x => x.Timestamp).ToList();
		return updatedList;
	}
}