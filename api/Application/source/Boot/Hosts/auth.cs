using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

public class AuthHostedService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;

    public AuthHostedService(IServiceProvider services, IConfiguration config)
    {
        _services = services;
        _config = config;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        using var scope  = _services.CreateScope();
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var appManager   = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var secret = _config["Strapi:ClientSecret"]
            ?? throw new InvalidOperationException("Strapi:ClientSecret is not configured.");

        // Scopes
        if (await scopeManager.FindByNameAsync("items.write", ct) is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "items.write",
                DisplayName = "Write access to items",
            }, ct);
        }

        // Clients
        if (await appManager.FindByClientIdAsync("strapi-cms", ct) is null)
        {
            await appManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "strapi-cms",
                ClientSecret = secret,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "items.write",
                }
            }, ct);
        }
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
