using Microsoft.IdentityModel.Tokens;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
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
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(config["OpenIddict:EncryptionKey"]!)));
                options.AddSigningKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(config["OpenIddict:SigningKey"]!)));
                // For dev only — use certificates in production instead of raw keys:
                // options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
                options.UseAspNetCore().EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            })
        ;

        return services;
    }
}
