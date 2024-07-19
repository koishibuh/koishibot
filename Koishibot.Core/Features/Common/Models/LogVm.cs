namespace Koishibot.Core.Features.Common.Models;

public record LogVm(string Message, DateTime Timestamp, SeverityLevel SeverityLevel);


public enum SeverityLevel : byte
{
	Info = 1,
	Warning = 2,
	Error = 3
}