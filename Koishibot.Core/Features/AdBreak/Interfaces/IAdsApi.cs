using Koishibot.Core.Features.AdBreak.Models;

namespace Koishibot.Core.Features.AdBreak.Interfaces;

public interface IAdsApi
{
    Task<AdBreakInfo> GetAdSchedule();
}
