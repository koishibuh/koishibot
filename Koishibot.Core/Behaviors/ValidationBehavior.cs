namespace Koishibot.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
	(IEnumerable<IValidator<TRequest>> validators)
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : class
{
	public async Task<TResponse> Handle
		(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancel)
	{
		ArgumentNullException.ThrowIfNull(next);

		// Are there any validators attached to the incoming request?
		if (validators.Any())
		{
			// If so, create a new validation context and validate
			var context = new ValidationContext<TRequest>(request);

			var validationResults = await Task.WhenAll(
				validators.Select(v =>
					v.ValidateAsync(context, cancel))).ConfigureAwait(false);

			// Filter out the failures where error count is greater than 0
			var failures = validationResults
				.Where(r => r.Errors.Count > 0)
				.SelectMany(r => r.Errors)
				.ToList();

			// Throw exception and pass error message
			if (failures.Count > 0)
				throw new FluentValidation.ValidationException(failures);
		}
		return await next().ConfigureAwait(false);
	}
}