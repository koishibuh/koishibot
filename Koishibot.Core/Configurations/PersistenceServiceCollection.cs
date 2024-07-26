using Koishibot.Core.Persistence;
using Microsoft.Extensions.Configuration;
namespace Koishibot.Core.Configurations;

public static class PersistenceServiceCollection
{
	public static IServiceCollection AddPersistenceServices
		(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("KoishibotConnectionString");
		var serverVersion = new MySqlServerVersion(new Version(10, 6, 18));

		services.AddDbContext<KoishibotDbContext>(options =>
			options.UseMySql(connectionString, serverVersion));

		services.AddSingleton<IAppCache, AppCache>();

		return services;
	}
}