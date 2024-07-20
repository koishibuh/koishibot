namespace Koishibot.Core.Features.Common.Models;

public record LogVm(string Message, DateTime Timestamp, string Level);


//public enum Level : byte
//{
//	Info = 1,
//	Warning = 2,
//	Error = 3
//}