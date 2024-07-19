using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Extensions;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.TwitchUsers;

// == ⚫ POST == //

public class AddTwitchUserController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Twitch User"])]
	[HttpPost("/api/twitch-user")]
	public async Task<ActionResult> AddTwitchUser
		([FromBody] AddTwitchUserCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record AddTwitchUserCommand(string Username) : IRequest
{
	public async Task<bool> IsUserUnique(KoishibotDbContext database)
	{
		var result = await database.Users
			.FirstOrDefaultAsync(x => x.Name == Username);

		return result is null;
	}
};

// == ⚫ HANDLER == //

public record AddTwitchUserHandler(
	IGetUserInfoByUsernameApi TwitchApi,
	KoishibotDbContext Database
	) : IRequestHandler<AddTwitchUserCommand>
{

	public async Task Handle
			(AddTwitchUserCommand c, CancellationToken cancel)
	{
		var result = await TwitchApi.GetUserInfoByUsername(c.Username);

		var twitchUser = new TwitchUser().Set(result);
		await Database.UpdateUser(twitchUser);
	}
}

// == ⚫ VALIDATOR == //

public class AddTwitchUserValidator
		: AbstractValidator<AddTwitchUserCommand>
{
	public KoishibotDbContext Database { get; set; }

	public AddTwitchUserValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.Username)
			.NotEmpty();

		RuleFor(p => p)
				.MustAsync(TwitchUserUnique)
				.WithMessage("Twitch User already exists");
	}

	private async Task<bool> TwitchUserUnique
			(AddTwitchUserCommand command, CancellationToken cancel)
	{
		return await command.IsUserUnique(Database);
	}
}

// == ⚫ TWITCH API == //

public record GetUserInfoByUsernameApi(
	ITwitchAPI TwitchApi,
	IRefreshAccessTokenService TokenProcessor
	) : IGetUserInfoByUsernameApi
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-users">Get Users Documentation</see> <br/>
	/// BroadcasterId, BroadcasterLogin, BroadcasterName<br/>
	/// Type (Admin, Global Mod, Staff, "" Normal User), Broadcaster Type (Affiliate, Partner, "" Normal)<br/>
	/// ChannelDescription, ProPic Url, Offline Url, Created At<br/>
	/// Up to 100 users in one request
	/// </summary>
	/// <returns></returns>
	public async Task<UserInfo> GetUserInfoByUsername(string username)
	{
		await TokenProcessor.EnsureValidToken();

		var users = new List<string> { username };

		var result = await TwitchApi.Helix.Users.GetUsersAsync(logins: users);
		return result is null || result.Users.Length == 0
			? throw new Exception("GetUserInfoByUsername returned null")
			: result.ConvertToDto();
	}
}
