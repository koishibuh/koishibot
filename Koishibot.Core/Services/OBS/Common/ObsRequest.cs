﻿using Koishibot.Core.Services.OBS.Enums;

namespace Koishibot.Core.Services.OBS.Common;

public class ObsRequest
{
	public OpCodeType? Op { get; set; } = OpCodeType.Request;

	[JsonPropertyName("d")]
	public RequestWrapper Data { get; set; } = null!;
}

public class RequestWrapper
{
	public string? RequestType { get; set; }
	public Guid RequestId { get; set; } = new Guid();
}

///
public class ObsRequest<T> 
{
	public OpCodeType? Op { get; set; } = OpCodeType.Request;

	[JsonPropertyName("d")]
	public RequestWrapper<T> Data { get; set; } = null!;
}

public class RequestWrapper<T>
{
	public string? RequestType { get; set; }
	public Guid RequestId { get; set; } = new Guid();
	public T? RequestData { get; set; }
}

///

public class ObsBatchRequest
{
	public OpCodeType? Op { get; set; } = OpCodeType.RequestBatch;

	[JsonPropertyName("d")]
	public RequestBatchWrapper Data { get; set; } = null;
}

public class RequestBatchWrapper
{
	public Guid RequestId { get; set; }
	public List<object> Requests { get; set; }
}