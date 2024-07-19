using Koishibot.Core.Features.StreamInformation.ViewModels;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Common.Models;


public record StreamInfo(
		TwitchUserDto Streamer,
		string Category,
		string CategoryId,
		string StreamTitle)
{
	public string StreamTitle { get; set; } = StreamTitle;

	public bool StreamOffline()
	{
		return this is null;
	}

	public StreamInfoVm ConvertToVm()
	{
		return new StreamInfoVm(StreamTitle, Category, CategoryId);
	}
}

