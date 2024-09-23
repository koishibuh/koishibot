using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.StreamInformation.Models;


// public class TwitchStream
// {
// 	public int Id { get; set; }
// 	public string StreamId { get; set; } = null!;
// 	public DateTimeOffset StartedAt { get; set; }
// 	public TimeSpan Duration { get; set; }
// 	public bool AttendanceMandatory { get; set; }
// 	public int YearlyQuarterId { get; set; }
//
// 	// == ⚫ == //
//
// 	public OutgoingRaid? OutgoingRaid { get; set; }
// 	public YearlyQuarter YearlyQuarter { get; set; } = null!;
//
// 	// == ⚫ == //
//
// 	// public async Task<TwitchStream> UpdateRepo(KoishibotDbContext database)
// 	// {
// 	// 	database.Update(this);
// 	// 	await database.SaveChangesAsync();
// 	//
// 	// 	return this;
// 	// }
//
// 	// public TwitchStream CalculateStreamDuration()
// 	// {
// 	// 	Duration = DateTime.UtcNow - StartedAt;
// 	// 	return this;
// 	// }
// }