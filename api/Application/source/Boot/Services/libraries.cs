using Mapster;

public static class LibraryServiceCollectionExtensions
{
    public static IServiceCollection AddLibraries(this IServiceCollection services, IConfiguration config)
    {
        services.AddMapster();

        return services;
    }
}
