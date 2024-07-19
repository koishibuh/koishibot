using Newtonsoft.Json;

namespace Koishibot.Core.Features.Lights.Models;

public class LightResponse
{
	[JsonProperty("data")]
	public List<DeviceInfo> Devices { get; set; }
}


public class DeviceInfo
{
	[JsonProperty("deviceType")]
	public int DeviceType { get; set; }

	[JsonProperty("ledVersionNum")]
	public int Version { get; set; }

	//[JsonProperty("moduleID")]
	//public string ModuleId { get; set; }

	[JsonProperty("macAddress")]
	public string MacAddress { get; set; } = string.Empty;

	//[JsonProperty("timeZoneID")]
	//public string TimeZoneId { get; set; }

	//[JsonProperty("dstOffset")]
	//public DateTimeOffset DateTimeOffset { get; set; }

	//[JsonProperty("rawOffset")]
	// public int RawrOffset {get; set;}

	[JsonProperty("deviceName")]
	public string DeviceName { get; set; } = string.Empty;

	//[JsonProperty("firmwareVer")]
	//public string FirmwareVersion { get; set; }	

	[JsonProperty("state")]
	public string State { get; set; } = string.Empty;

	//[JsonProperty("routerSSID")]
	//public string RouterId { get; set; } = string.Empty;

	[JsonProperty("localIP")] 
	public string LocalIp { get; set; } = string.Empty;

	//[JsonProperty("routerRssi")]
	//public int RouterRssi { get; set; }

	[JsonProperty("isOnline")]
	public bool IsOnline { get; set; }	
}
