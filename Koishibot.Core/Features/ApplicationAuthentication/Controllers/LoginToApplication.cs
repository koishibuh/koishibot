using Koishibot.Core.Features.ApplicationAuthentication.Models;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Koishibot.Core.Features.ApplicationAuthentication.Controllers;

// == ⚫ POST == //

public class LoginToApplicationController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Application"])]
	[HttpPost("/api/login")]
	[AllowAnonymous]
	public async Task<ActionResult> LoginToApplication
		([FromBody] LoginToApplicationCommand dto)
	{
		await Mediator.Send(dto);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record LoginToApplicationCommand(
	string Username,
	string Password
	) : IRequest<JwtVm>;

// == ⚫ HANDLER == //

public record LoginToApplicationHandler(
	IOptions<Settings> Settings,
	IAppCache Cache, KoishibotDbContext Database
	) : IRequestHandler<LoginToApplicationCommand, JwtVm>
{
	public async Task<JwtVm> Handle
		(LoginToApplicationCommand command, CancellationToken cancel)
	{
		var appAuth = Settings.Value.AppAuthentication;

		var result = await Database.GetLoginByUsername(command.Username)
			?? throw new Exception("Invalid Login");

		var validPassword = BCrypt.Net.BCrypt.Verify(command.Password, result.HashedPassword);
		if (validPassword is false) { throw new Exception("Invalid Login"); }

		var key = Encoding.UTF8.GetBytes(appAuth.Key);
		var signingCredentials = new SigningCredentials
			(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

		var jwtToken = new JwtSecurityToken(
			appAuth.Issuer,
			appAuth.Audience,
			new List<Claim> { new("username", result.Username ) },
			DateTime.UtcNow,
			DateTime.UtcNow.AddHours(12),
			signingCredentials
		);

		var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
		return new JwtVm(token, jwtToken.Claims.ToList());
	}
}

// == ⚫ EXTENSIONS == //

public static class LoginToApplication
{
	public static async Task<AppLogin?> GetLoginByUsername
		(this KoishibotDbContext database, string username)
	{
		return await database.AppLogins
			.Where(x => x.Username == username)
			.FirstOrDefaultAsync();
	}
}