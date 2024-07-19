using Koishibot.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var app = builder
.ConfigureServices()
.ConfigurePipline();

app.UseSerilogRequestLogging();

await app.MigrateDatabase();

app.Run();