using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Koishibot.Core.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
	: IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync
		(HttpContext httpContext, Exception exception, CancellationToken cancel)
	{
		// Create a problemdetails object and add current path as instance property
		var problemDetails = new ProblemDetails();
		problemDetails.Instance = httpContext.Request.Path;

		if (exception is FluentValidation.ValidationException fluentException)
		{
			// Set the properties of the problem details object, append errors, set status code to 400
			problemDetails.Title = "one or more validation errors occurred.";
			problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
			httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			List<string> validationErrors = new List<string>();
			foreach (var error in fluentException.Errors)
			{
				validationErrors.Add(error.ErrorMessage);
			}
			problemDetails.Extensions.Add("errors", validationErrors);
		}
		else if (exception is BaseException e)
		{
			httpContext.Response.StatusCode = (int)e.StatusCode;
			problemDetails.Title = e.Message;
		}
		else
		{
			problemDetails.Title = exception.Message;
		}

		logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

		problemDetails.Status = httpContext.Response.StatusCode;
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancel).ConfigureAwait(false);
		return true;
	}
}