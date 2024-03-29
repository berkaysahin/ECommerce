using ECommerce.Gateway.DelegateHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();
    
    builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
    {
        options.Authority = builder.Configuration["IdentityServerURL"];
        options.Audience = "resource_gateway";
        options.RequireHttpsMetadata = false;
    });
    
    var env = hostingContext.HostingEnvironment;
    config.AddJsonFile($"configuration.{env.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();
});

builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>();

var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();

app.Run();
