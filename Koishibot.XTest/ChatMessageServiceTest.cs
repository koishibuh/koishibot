using HandlebarsDotNet;
using Koishibot.Core.Features.ChatCommands;
namespace Koishibot.XTest;

public class ChatMessageServiceTest : IChatReplyService
{
	public string Message = "";

	public Task Everyone<T>(string command, T data) => throw new NotImplementedException();
	public Task Start<T>(string command, T data, string permission) => throw new NotImplementedException();

	public async Task OLDAPP(string command)
	{
		Message = command;
		await Task.CompletedTask;
	}
	public async Task OLDAPP<T>(string command, T data)
	{
		Message = command;
		await Task.CompletedTask;
	}
	public Task CreateResponse(string command) => throw new NotImplementedException();
	public Task CreateResponse<T>(string command, T data) => throw new NotImplementedException();


}