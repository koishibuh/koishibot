namespace Koishibot.Core.Services.OBS.Sources;

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getsourcescreenshot">Obs Documentation</see>
/// </summary>
public class GetSourceScreenshotRequest
{
	/// <summary>
	/// Name of the source to take a screenshot of
	/// </summary>
	public string SourceName { get; set; } = string.Empty;

	/// <summary>
	/// UUID of the source to take a screenshot of
	/// </summary>
	public string SourceUuid { get; set; } = string.Empty;

	/// <summary>
	/// Image compression format to use. Use GetVersion to get compatible image formats
	/// </summary>
	public string ImageFormat {  get; set; } = string.Empty;

	/// <summary>
	/// Width to scale the screenshot to (>= 8, <= 4096)
	/// </summary>
	public int ImageWidth { get; set; }

	/// <summary>
	/// Height to scale the screenshot to (>= 8, <= 4096)
	/// </summary>
	public int ImageHeight { get; set; }

	/// <summary>
	/// Compression quality to use. 0 for high compression, 100 for uncompressed. -1 to use "default"
	/// </summary>
	public int ImageCompressionQuality { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetSourceScreenshotResponse
{
	/// <summary>
	/// Base64-encoded screenshot
	/// </summary>
	public string? ImageData { get; set; }
}