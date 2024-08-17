using System.IdentityModel.Tokens.Jwt;
using Koishibot.Core.Services.StreamElements;
namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream-elements")]
public class ConnectStreamElementsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["StreamElements"])]
	[HttpPost]
	public async Task<ActionResult> ConnectStreamElements()
	{
		await Mediator.Send(new ConnectStreamElementsCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ConnectStreamElementsHandler(
IOptions<Settings> Settings,
IStreamElementsService StreamElementsService
) : IRequestHandler<ConnectStreamElementsCommand>
{
	public async Task Handle
		(ConnectStreamElementsCommand command, CancellationToken cancel)
	{
		// TODO: Send status to UI when connected
		// TODO: Alert on UI to update JWT Token
		if (TokenIsValid())
		{
			await StreamElementsService.CreateWebSocket();
		}
		else
		{
			throw new Exception("Jwt Token has expired");
		}
	}

	private bool TokenIsValid()
	{
		var token = Settings.Value.StreamElementsJwtToken;
		var tokenTicks = GetTokenExpirationTime(token);
		var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

		var now = DateTime.Now.ToUniversalTime();
		var valid = tokenDate >= now;
		return valid;
	}

	private long GetTokenExpirationTime(string token)
	{
		var handler = new JwtSecurityTokenHandler();
		var jwtSecurityToken = handler.ReadJwtToken(token);
		var tokenExpiration = jwtSecurityToken.Claims.First(x => x.Type.Equals("exp")).Value;
		return long.Parse(tokenExpiration);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ConnectStreamElementsCommand : IRequest;