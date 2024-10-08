using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Koishibot.Core.Services.Websockets;

public record WebSocketMessage(
Guid Id,
string Message)
{
	public bool IsNullOrEmpty()
		=> string.IsNullOrEmpty(Message);

	public bool IsPing()
		=> Message.StartsWith("0{\"sid\"") || Message.Contains("PING");

	public bool IsConnected()
		=> Message.StartsWith("40"); // Message Connect

	public bool IsAuthenticated()
		=> Message.Contains("authenticated");

	public bool IsUnauthorized()
		=> Message.Contains("unauthorized");

	public bool IsEvent()
		=> Message.StartsWith("42");

	public bool IsEventUpdate()
		=> Message.Contains("event:update") || Message.Contains("stream:update");


	public string ParseResponse()
	{
		var trimmed = Regex.Replace(Message, @"^\d+", "");
		var doc = JsonDocument.Parse(trimmed);
		var root = doc.RootElement;
		return root[1].GetRawText();
	}

	public JsonObject ConvertToJsonObject()
		=> JsonNode.Parse(Message)?.AsObject()
			?? throw new Exception("cry" + Message);
}