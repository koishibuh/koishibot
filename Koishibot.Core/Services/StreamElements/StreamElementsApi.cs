//using Koishibot.Core.Services.Websockets;
//namespace Koishibot.Core.Services.StreamElements;

//public interface IStreamElementsApi
//{
//	Task CreateWebSocket();
//}

//public class StreamElementsApi
//{
//	private CancellationToken? _cancel;

//	private string jwt = "";

//	public async Task CreateWebSocket()
//	{
//		_cancel ??= new CancellationToken();
//		client ??= new StreamElementsWebSocketHandler
//			("wss://realtime.streamelements.com/socket.io/?cluster=main&EIO=3&transport=websocket",
//				_cancel.Value, 3);

//		client.MessageReceived += async message =>
//		{
//			var text = message;
//			if (message.Contains("\""))
//			{
//				string text2 = message.Split('"', StringSplitOptions.None)[0].Substring(0, message.Split('"', StringSplitOptions.None)[0].Length - 1);
//				text = message.Substring(text2.Length);
//			}

//			if (message.StartsWith("0{\"sid\"")) // // IsPingMessage()
//			{
//				//await _logClient.LogMessage(new LogMessage("PING received from Twitch.", DateTime.UtcNow,
//				//		SeverityLevel.Info));
//				await client.SendMessage("2");
//				//await _logClient.LogMessage(new LogMessage("PONG :tmi.twitch.tv sent back to Twitch.", DateTime.UtcNow,
//				//		SeverityLevel.Info));
//			}

//			if (message.StartsWith("40"))
//			{
//				await Authenticate(jwt);
//				return;
//			}

//			if (message.StartsWith("42[\"authenticated\""))
//			{
//				//on authenticated
//				return;
//			}

//			if (message.StartsWith("42[\"unauthorized\""))
//			{
//				//on failure
//				return;
//			}

//			if (message.StartsWith("42[\"event\",{\"_id\""))
//			{
//				//on handle complex
//				return;
//			}
//			if (message.StartsWith("42[\"event:update\",{\"name\""))
//			{
//				//on simple update
//				return;
//			}


//		};

//	}

//	public async Task Authenticate(string jwt)
//	{
//		await client.SendMessage("42[\"authenticate\",{\"method\":\"jwt\",\"token\":\"" + jwt + "\"}]");
//	}

//	private StreamElementsWebSocketHandler client;




//	public event EventHandler OnConnected;


//	public event EventHandler OnDisconnected;

//	public event EventHandler<ErrorEventArgs> OnError;

//	public event EventHandler<string> OnSent;

//	public event EventHandler<string> OnReceivedRawMessage;

//	public event EventHandler<Authenticated> OnAuthenticated;

//	public event EventHandler OnAuthenticationFailure;

//	public event EventHandler<Tip> OnTip;


//	//private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
//	//{
//	//	this.OnReceivedRawMessage?.Invoke(client, e.Message);
//	//	string text = e.Message;
//	//	if (text.Contains("\""))
//	//	{
//	//		string text2 = e.Message.Split('"', StringSplitOptions.None)[0].Substring(0, e.Message.Split('"', StringSplitOptions.None)[0].Length - 1);
//	//		text = e.Message.Substring(text2.Length);
//	//	}

//	//	if (e.Message.StartsWith("40"))
//	//	{
//	//		handleAuthentication();
//	//		return;
//	//	}

//	//	if (e.Message.StartsWith("0{\"sid\""))
//	//	{
//	//		handlePingInitialization(Internal.handleSessionMetadata(JObject.Parse(text)));
//	//	}

//	//	if (e.Message.StartsWith("42[\"authenticated\""))
//	//	{
//	//		this.OnAuthenticated?.Invoke(client, Internal.handleAuthenticated(JArray.Parse(text)));
//	//		return;
//	//	}

//	//	if (e.Message.StartsWith("42[\"unauthorized\""))
//	//	{
//	//		this.OnAuthenticationFailure?.Invoke(client, null);
//	//	}

//	//	if (e.Message.StartsWith("42[\"event\",{\"_id\""))
//	//	{
//	//		handleComplexObject(JArray.Parse(text));
//	//	}
//	//	else if (e.Message.StartsWith("42[\"event:update\",{\"name\""))
//	//	{
//	//		handleSimpleUpdate(JArray.Parse(text));
//	//	}
//	//}

//	//private void handleComplexObject(JArray decoded)
//	//{
//	//	if (!(decoded[0].ToString() != "event") && !(decoded[1]["provider"].ToString() != "twitch"))
//	//	{
//	//		switch (decoded[1]["type"].ToString())
//	//		{
//	//			case "tip":
//	//				this.OnTip?.Invoke(client, ParseTip.handleTip(decoded[1]["data"]));
//	//				break;
//	//			default:
//	//				break;
//	//		}
//	//	}
//	//}


//}
