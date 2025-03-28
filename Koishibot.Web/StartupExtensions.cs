﻿using AspNetCore.Authentication.ApiKey;
using Koishibot.Core.Common;
using Koishibot.Core.Configurations;
using Koishibot.Core.Exceptions;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services;
using Koishibot.Core.Services.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace Koishibot.Web;

public static class StartupExtensions
{
	public static WebApplication ConfigureServices
		(this WebApplicationBuilder builder)
	{
		var debugMode = builder.Environment.IsDevelopment();
		if (debugMode)
		{
			builder.Configuration.AddJsonFile
				("ASettings/settings.json", optional: false);
			builder.Configuration.AddJsonFile
				("ASettings/dbstring.json", optional: false);


			//var googleTokens = appSettings.GetRequiredSection("GoogleTokens");
			//builder.Services.AddSingleton(s =>
			//{
			//	var credential = new ServiceAccountCredential
			//		(new ServiceAccountCredential.Initializer(googleTokens.GetRequiredSection("ClientEmail").Value)
			//	{
			//		Scopes = new[] { CalendarService.Scope.Calendar }
			//	}.FromPrivateKey(googleTokens.GetRequiredSection("PrivateKey").Value));

			//	return new CalendarService(new BaseClientService.Initializer()
			//	{
			//		HttpClientInitializer = credential,
			//		ApplicationName = googleTokens.GetRequiredSection("ApplicationName").Value
			//	});
			//});

		}
		else
		{
			builder.Configuration.AddEnvironmentVariables();
		}

		var appSettings = builder.Configuration.GetSection("AppSettings");
		builder.Services.Configure<Settings>(appSettings);
		var auth = appSettings.GetRequiredSection("AppAuthentication");
		
		builder.Services.AddAuthentication()
			.AddJwtBearer(o =>
			{
				o.TokenValidationParameters = new()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = auth.GetRequiredSection("Issuer").Value,
					ValidAudience = auth.GetRequiredSection("Audience").Value,
					IssuerSigningKey = new SymmetricSecurityKey
						(Encoding.ASCII.GetBytes
							(auth.GetRequiredSection("Key").Value))
				};
			})
			.AddApiKeyInHeaderOrQueryParams(options =>
			{
				options.Realm = "Bot Api";
				options.KeyName = "API-Key";
				options.Events = new ApiKeyEvents
				{
					OnValidateKey = async context =>
					{
						var validApiKey = auth.GetRequiredSection("EGKey").Value;

						if (context.ApiKey == validApiKey)
						{
							context.ValidationSucceeded();
						}
						else
						{
							context.ValidationFailed();
						}

						await Task.CompletedTask;
					}
				};
			});

		builder.Services.AddAuthorization(options =>
		{
			options.DefaultPolicy = new AuthorizationPolicyBuilder()
				.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyDefaults.AuthenticationScheme)
				.RequireAuthenticatedUser()
				.Build();
		});

		builder.Services.AddHttpClient("Dictionary", httpClient =>
			httpClient.BaseAddress = new Uri("https://api.dictionaryapi.dev/api/v2/entries/en/"));

		builder.Services.AddHttpClient("MagicHue", httpClient =>
			httpClient.BaseAddress = new Uri("https://wifij01us.magichue.net/app/"));

		builder.Services.AddHttpClient("Twitch", httpClient =>
			httpClient.BaseAddress = new Uri("https://api.twitch.tv/helix/"));

		builder.Services.AddHttpClient("TwitchTest", httpClient =>
			httpClient.BaseAddress = new Uri("http://localhost:8080/"));

		builder.Services.AddHttpClient("Wordpress", httpClient =>
			httpClient.BaseAddress = new Uri("https://www.elysiagriffin.com/wp-json/wp/v2/"));

		builder.Services.AddHttpClient("Default");

		builder.Services.AddSerilog((services, lc) => lc
			.ReadFrom.Configuration(builder.Configuration)
			.ReadFrom.Services(services)
			//.Enrich.FromLogContext()
			.WriteTo.Console());

		builder.Services.AddHostedService<KoishibotBackgroundWorker>();

		builder.Services.AddAppServices();
		builder.Services.AddInfrastructureServices(builder.Configuration);
		builder.Services.AddPersistenceServices(builder.Configuration);

		builder.Services.AddMemoryCache();

		builder.Services.AddSignalR();

		builder.Services.AddCors(options =>
		{
			options.AddPolicy("LocalPolicy", b => b
				.AllowAnyOrigin()
				//.WithOrigins("http://localhost:5210", "http://localhost:5000")
				.AllowAnyHeader()
				.AllowAnyMethod());
		});

		builder.Services.AddControllers();

		var koishibotHubUrl = debugMode
			? "https://localhost:7115/notifications"
			: "http://localhost:8080/notifications";

		var koishibotHubConnection = new HubConnectionBuilder()
			.WithUrl(koishibotHubUrl)
			.WithAutomaticReconnect()
			.Build();

		builder.Services.AddKeyedSingleton("notifications", koishibotHubConnection);

		builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

		builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
		builder.Services.AddProblemDetails();

		return builder.Build();
	}

	public static WebApplication ConfigurePipline
		(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		var forwardedHeaderOptions = new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
			RequireHeaderSymmetry = false
		};

		forwardedHeaderOptions.KnownNetworks.Clear();
		forwardedHeaderOptions.KnownProxies.Clear();
		app.UseForwardedHeaders(forwardedHeaderOptions);

		app.UseExceptionHandler();

		app.UseStaticFiles();

		app.UseRouting();

		app.UseCors("LocalPolicy");

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
		app.MapHub<SignalrHub>("/notifications");
		app.MapFallbackToFile("index.html");

		return app;
	}

	public static async Task MigrateDatabase(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		try
		{
			var context = scope.ServiceProvider.GetService<KoishibotDbContext>();
			if (context != null)
			{
				await context.Database.MigrateAsync();
			}
		}
		catch (Exception)
		{
			//add logging here later on
		}
	}
}