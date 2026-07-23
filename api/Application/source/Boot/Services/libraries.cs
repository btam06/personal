using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class LibraryServiceCollectionExtensions
{
    public static IServiceCollection AddLibraries(this IServiceCollection services, IConfiguration config)
    {
        services.AddMapster();

        return services;
    }
}
