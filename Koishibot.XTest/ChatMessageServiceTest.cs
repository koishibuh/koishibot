using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands;
namespace Koishibot.XTest;

public class ChatMessageServiceTest : IChatReplyService
{
	public string Message = "";

	public Task Everyone<T>(string command, T data) => throw new NotImplementedException();
	public Task Start<T>(string command, T data, string permission) => throw new NotImplementedException();

	public async Task App(string command)
	{
		Message = command;
		await Task.CompletedTask;
	}
	public async Task App<T>(string command, T data)
	{
		Message = command;
		await Task.CompletedTask;
	}
}