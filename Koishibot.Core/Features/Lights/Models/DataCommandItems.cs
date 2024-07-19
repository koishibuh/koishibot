namespace Koishibot.Core.Features.Lights.Models;


public class Payload
{
	public DataCommandItem[]? dataCommandItems { get; set;}
}


public class DataCommandItem
{
	public string hexData { get; set; } = string.Empty;
	public string macAddress { get; set; } = string.Empty;
}