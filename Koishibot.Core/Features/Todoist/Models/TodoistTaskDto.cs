using Koishibot.Core.Features.Todoist.Enums;

namespace Koishibot.Core.Features.Todoist.Models;

public record TodoistTaskDto(
	string Username,
	string Message,
	TaskType Type
	);
