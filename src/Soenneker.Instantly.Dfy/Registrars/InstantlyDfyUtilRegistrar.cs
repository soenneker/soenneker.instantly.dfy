using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Instantly.Dfy.Abstract;
using Soenneker.Instantly.ClientUtil.Registrars;

namespace Soenneker.Instantly.Dfy.Registrars;

/// <summary>
/// A .NET typesafe implementation of Instantly.ai's DFY API
/// </summary>
public static class InstantlyDfyUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IInstantlyDfyUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddInstantlyDfyUtilAsSingleton(this IServiceCollection services)
    {
        services.AddInstantlyOpenApiClientUtilAsSingleton()
                .TryAddSingleton<IInstantlyDfyUtil, InstantlyDfyUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IInstantlyDfyUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddInstantlyDfyUtilAsScoped(this IServiceCollection services)
    {
        services.AddInstantlyOpenApiClientUtilAsSingleton()
                .TryAddScoped<IInstantlyDfyUtil, InstantlyDfyUtil>();

        return services;
    }
}
