using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.ChannelPoints.Controllers;

public record GetDragonPage();

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest/")]
public class GetLiveStreamsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Egg Quest"])]
	[HttpGet("page")]
	public async Task<ActionResult> GetDragonPage()
	{
		await Mediator.Send(new GetDragonPageQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetDragonPageHandler(
IOptions<Settings> Settings,
IHttpClientFactory HttpClientFactory
) : IRequestHandler<GetDragonPageQuery>
{
	//TODO: Save location temporarily in cache
	public async Task Handle(GetDragonPageQuery query, CancellationToken cancel)
	{
		var httpClient = HttpClientFactory.CreateClient("Default");
		var url = "https://dragcave.net/locations/2-desert";

		using var response = await httpClient.GetAsync(url, cancel);
		response.EnsureSuccessStatusCode();

		var content = await response.Content.ReadAsStringAsync(cancel);
		var webpage = new HtmlDocument();
		webpage.LoadHtml(content);

		var eggNodes = webpage.DocumentNode.SelectNodes("//div[@class='eggs']//div")
		               ?? throw new Exception("Error finding egg on page");

		var eggDescriptions = eggNodes
			.Select(egg => egg.SelectSingleNode(".//span")
				?? throw new Exception("Error finding egg description"))
			.Select(span => span.InnerText)
			.ToList();
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetDragonPageQuery : IRequest;