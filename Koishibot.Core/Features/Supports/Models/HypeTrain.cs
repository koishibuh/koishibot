using Koishibot.Core.Features.ChatCommands.Extensions;

namespace Koishibot.Core.Features.Supports.Models;

public class HypeTrain : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset StartedAt { get; set; }
	public DateTimeOffset EndedAt { get; set; }
	public int LevelCompleted { get; set; }
}
