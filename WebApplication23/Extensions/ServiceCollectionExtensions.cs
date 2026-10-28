using MovieApp.Services;

namespace MovieApp.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Реєструє сервіси прикладного рівня в DI-контейнері.</summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Scoped, бо MovieService залежить від DbContext (він теж Scoped)
        services.AddScoped<IMovieService, MovieService>();
        return services;
    }
}
