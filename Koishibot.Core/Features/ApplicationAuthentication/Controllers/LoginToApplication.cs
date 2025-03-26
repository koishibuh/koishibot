using Koishibot.Core.Features.ApplicationAuthentication.Models;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Koishibot.Core.Exceptions;

namespace Koishibot.Core.Features.ApplicationAuthentication.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/login")]
public class LoginToApplicationController : ApiControllerBase
{
	[HttpPost]
	[AllowAnonymous]
	public async Task<ActionResult> LoginToApplication
	([FromBody] LoginToApplicationCommand dto)
	{
		var response = await Mediator.Send(dto);
		return Ok(response);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record LoginToApplicationHandler(
IOptions<Settings> Settings,
KoishibotDbContext Database
) : IRequestHandler<LoginToApplicationCommand, JwtVm>
{
	public async Task<JwtVm> Handle
	(LoginToApplicationCommand command, CancellationToken cancel)
	{
		var appAuth = Settings.Value.AppAuthentication;

		var result = await Database.GetLoginByUsername(command.Username)
		             ?? throw new Exception("Invalid Login");

		var validPassword = BCrypt.Net.BCrypt.Verify(command.Password, result.Value);
		if (validPassword is false)
			throw new CustomException("Invalid Login");

		var key = Encoding.UTF8.GetBytes(appAuth.Key);
		var signingCredentials = new SigningCredentials
		(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

		var jwtToken = new JwtSecurityToken(
		appAuth.Issuer,
		appAuth.Audience,
		new List<Claim> { new("username", result.Key) },
		DateTime.UtcNow,
		DateTime.UtcNow.AddHours(12),
		signingCredentials
		);

		var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
		return new JwtVm(token, jwtToken.Claims.ToList());
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record LoginToApplicationCommand(
string Username,
string Password
) : IRequest<JwtVm>;

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class LoginToApplication
{
	// public static async Task<AppLogin?> GetLoginByUsername
	// (this KoishibotDbContext database, string username)
	// 	=> await database.AppLogins.FirstOrDefaultAsync(x => x.Username == username);
	
	public static async Task<AppKey?> GetLoginByUsername
	(this KoishibotDbContext database, string username)
		=> await database.AppKeys.FirstOrDefaultAsync(x => x.Name == "AppLogin" && x.Key == username);
}