using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;
    config.AddJsonFile($"configuration.{env.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();
});

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run();
