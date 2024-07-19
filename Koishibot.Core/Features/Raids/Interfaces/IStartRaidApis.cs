namespace Koishibot.Core.Features.Raids.Interfaces;
public interface IStartRaidApis
{
	Task UpdateStreamTitle(string title);
	Task<DateTime?> GetNextScheduledStreamDate();
}