namespace Koishibot.Core.Services.OBS.Sources;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetInputKindListHandler() : IRequestHandler<GetInputKindListCommand>
{
	public async Task Handle(GetInputKindListCommand request, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO: WIP
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record GetInputKindListCommand(GetInputKindListResponse args) : IRequest;


/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getinputkindlist">Obs Documentation</see>
/// </summary>
public class GetInputKindListRequest
{
	/// <summary>
	/// True == Return all kinds as unversioned,<br/>
	/// False == Return with version suffixes (if available)
	/// </summary>
	public bool Unversioned { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetInputKindListResponse
{
	/// <summary>
	/// Array of input kinds
	/// </summary>
	public List<string>? InputKinds { get; set; }
}