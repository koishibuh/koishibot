using Koishibot.Core.Features.AdBreak;
using Scrutor;

namespace Koishibot.Core.Configurations;

public static class ApplicationServiceCollection
{
	public static IServiceCollection AddAppServices
			(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollection).Assembly);

		services.AddMediatR(cfg =>
		{
			// Order matters
			cfg.RegisterServicesFromAssemblies(typeof(ApplicationServiceCollection).Assembly);
			//cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
			cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});

		services.AddSingleton<IPomodoroTimer, PomodoroTimer>();

		services.Scan(selector => selector.FromAssemblies(typeof(ApplicationServiceCollection).Assembly)
		.AddClasses(false)
		.UsingRegistrationStrategy(RegistrationStrategy.Skip)
		.AsMatchingInterface()
		.WithTransientLifetime());

		return services;
	}
}