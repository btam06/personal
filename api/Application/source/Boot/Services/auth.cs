using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<AppDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token");
                options.AllowClientCredentialsFlow();
                /*options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(config["OpenIddict:EncryptionKey"]!)
                ));
                options.AddSigningKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(config["OpenIddict:SigningKey"]!)
                ));*/
                // For dev only — use certificates in production instead of raw keys:
                options.UseAspNetCore().EnableTokenEndpointPassthrough();

                if (env.IsDevelopment())
                {
                    options.UseAspNetCore().DisableTransportSecurityRequirement();
                    options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
                }
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            })
        ;

        // Scopes
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ItemsWrite", policy => policy.RequireClaim("scope", "items.write"));
        });

        services.AddHostedService<AuthHostedService>();

        return services;
    }
}
