using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.Supports.Extensions;
public static class SupportExtensions
{
	// DATABASE

	public static async Task AddFollow(this KoishibotDbContext database, ChannelFollow follow)
	{
		database.Update(follow);
		await database.SaveChangesAsync();
	}

	public static async Task AddCheer(this KoishibotDbContext database, TwitchCheer cheer)
	{
		database.Update(cheer);
		await database.SaveChangesAsync();
	}

	public static async Task UpdateGiftSubTotal
		(this KoishibotDbContext database, int userId, int giftSubtotal)
	{
		var result = await database.SupportTotals
			.Where(x => x.UserId == userId)
			.FirstOrDefaultAsync();

		if (result is null)
		{
			result = new SupportTotal
			{
				UserId = userId,
				MonthsSubscribed = 0,
				SubsGifted = giftSubtotal,
				BitsCheered = 0,
				Tipped = 0
			};
		}	else
		{
			result.SubsGifted = giftSubtotal;
		}

		database.Update(result);
		await database.SaveChangesAsync();
	}


	public static async Task UpdateCheerTotal
		(this KoishibotDbContext database, TwitchCheer cheer)
	{
		var result = await database.SupportTotals
			.Where(x => x.UserId == cheer.UserId)
			.FirstOrDefaultAsync();

		if (result is null)
		{
			result = new SupportTotal
			{
				UserId = cheer.UserId,
				MonthsSubscribed = 0,
				SubsGifted = 0,
				BitsCheered = cheer.BitsAmount,
				Tipped = 0
			};
		}
		else
		{
			result.BitsCheered = result.BitsCheered + cheer.BitsAmount;
		}

		database.Update(result);
		await database.SaveChangesAsync();
	}

	public static async Task UpdateSubDurationTotal
		(this KoishibotDbContext database, Subscription sub, int duration)
	{
		var result = await database.SupportTotals
			.Where(x => x.UserId == sub.UserId)
			.FirstOrDefaultAsync();

		if (result is null)
		{
			result = new SupportTotal
			{
				UserId = sub.UserId,
				MonthsSubscribed = duration,
				SubsGifted = 0,
				BitsCheered = 0,
				Tipped = 0
			};
		}
		else
		{
			if (result.MonthsSubscribed == 0)
			{
				result.MonthsSubscribed = 1;
			} else
			{
				result.MonthsSubscribed = result.MonthsSubscribed + 1;
			}
		}

		database.Update(result);
		await database.SaveChangesAsync();
	}
}