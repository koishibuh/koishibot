﻿namespace Koishibot.Core.Features.Common.Models;

public record LogVm(
	string Message, 
	string Level,
	DateTime Timestamp
	)
{
	public LogVm(string Message, string Level, DateTime? Timestamp = null)
		: this(Message, Level, Timestamp ?? DateTime.UtcNow) { }
};




//public enum Level : byte
//{
//	Info = 1,
//	Warning = 2,
//	Error = 3
//}