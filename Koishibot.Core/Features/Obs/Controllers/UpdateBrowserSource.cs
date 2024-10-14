using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Obs.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Sources;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
[AllowAnonymous]
public class UpdateBrowserSourceController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpPost("source")]
	public async Task<ActionResult> UpdateBrowserSource()
	{
		await Mediator.Send(new UpdateBrowserSourceCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record UpdateBrowserSourceHandler(
IObsService ObsService,
KoishibotDbContext Database,
IAppCache Cache
) : IRequestHandler<UpdateBrowserSourceCommand>
{
	public async Task Handle
		(UpdateBrowserSourceCommand c, CancellationToken cancel)
	{
		var result = Cache.GetStatusByServiceName(ServiceName.ObsWebsocket);
		if (result is false)
		{
			throw new CustomException("ObsWebsocket not connected");
		}

		var browserSource = await Database.FindObsItemByAppName(ObsAppName.ShoutoutVideo);
		if (browserSource is null)
		{
			throw new CustomException("ShoutoutVideo ObsSource not found");
		}
		else
		{
			// update input settings
			var request = new RequestWrapper<SetInputSettingsRequest<BrowserSourceSettings>>
			{
				RequestType = ObsRequests.SetInputSettings,
				RequestId = new Guid(),
				RequestData = new SetInputSettingsRequest<BrowserSourceSettings>
				{
					InputUuid = browserSource.ObsId,
					InputSettings = new BrowserSourceSettings
					{
						Url = "https://www.google.com"
					}
				}
			};

			var inputRequest = new RequestWrapper<OpenInputInteractDialog>
			{
				RequestType = ObsRequests.OpenInputInteractDialog,
				RequestId = new Guid(),
				RequestData = new OpenInputInteractDialog
				{
					InputUuid = browserSource.ObsId
				}
			};


			var requests2 = new List<Object>
			{
				inputRequest,
				request
			};

			var test = new ObsBatchRequest
			{
				Data = new RequestBatchWrapper
				{
					RequestId = new Guid(),
					Requests = requests2
				}
			};

			await ObsService.SendBatchRequest(test);
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record UpdateBrowserSourceCommand : IRequest;