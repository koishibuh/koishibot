using HtmlAgilityPack;
using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
namespace Koishibot.Core.Features.KoiKinDragons.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest/")]
public class GetDragonPageController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Egg Quest"])]
	[HttpPost("page")]
	public async Task<ActionResult> GetDragonPage([FromBody] GetDragonPageQuery e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetDragonPageHandler(
IAppCache Cache,
IHttpClientFactory HttpClientFactory,
IChatReplyService ChatReplyService
) : IRequestHandler<GetDragonPageQuery, object>
{
	public async Task<object> Handle(GetDragonPageQuery query, CancellationToken cancel)
	{
		var winner = Cache.GetDragonQuestWinner();
		if (winner is null)
			throw new CustomException("DragonQuest Winner is null");

		var webpage = await FetchWebpage(query.location);
		var eggDescriptions = GetDescriptions(webpage);

		var template = CreateTemplate(winner.Name, eggDescriptions);

		await ChatReplyService.App(Command.DragonQuestPickEgg, template);
		return template;
		// TODO: display descriptions on screen?
	}

	/*═══════════════════【】═══════════════════*/
	private async Task<HtmlDocument> FetchWebpage(string url)
	{
		var httpClient = HttpClientFactory.CreateClient("Default");
		using var response = await httpClient.GetAsync(url);
		response.EnsureSuccessStatusCode();

		var content = await response.Content.ReadAsStringAsync();
		var webpage = new HtmlDocument();
		webpage.LoadHtml(content);
		return webpage;
	}

	private List<string> GetDescriptions(HtmlDocument webpage)
	{
		var eggNodes = webpage.DocumentNode.SelectNodes("//div[@class='eggs']//div")
			?? throw new Exception("Error finding egg on page");

		return eggNodes
			.Select(egg => egg.SelectSingleNode(".//span")
				?? throw new Exception("Error finding egg description"))
			.Select(span => span.InnerText)
			.ToList();
	}

	private object CreateTemplate(string winner, List<string> eggDescriptions) => new
		{
			User = winner,
			Choice1 = eggDescriptions[0] ?? string.Empty,
			Choice2 = eggDescriptions[1] ?? string.Empty,
			Choice3 = eggDescriptions[2] ?? string.Empty
		};
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetDragonPageQuery(string location) : IRequest<object>;