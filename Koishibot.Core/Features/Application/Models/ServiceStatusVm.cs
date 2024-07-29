namespace Koishibot.Core.Features.Application.Models;

public record ServiceStatusVm(string Name, bool Status)
{
	public bool Status { get; set; } = Status;
};

