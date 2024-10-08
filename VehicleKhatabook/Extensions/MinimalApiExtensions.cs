
using System.Reflection;
using VehicleKhatabook.Infrastructure;

namespace VehicleKhatabook.Extensions
{
    public static class MinimalApiExtensions
    {
        /// <summary>
        /// Extension method to execute DefineServices on each endpoint class in the calling assembly only
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAllMinimalApiDefinitions(this IServiceCollection services, IConfiguration configuration)
        {
            List<IEndpointDefinition> endpointDefinitions = new();

            endpointDefinitions.AddRange(Assembly.GetCallingAssembly().ExportedTypes
                .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>());

            foreach (var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.DefineServices(services, configuration);                    
            }

            services.AddSingleton<IReadOnlyCollection<IEndpointDefinition>>(endpointDefinitions);			
		}

        /// <summary>
        /// Extension method to execute DefineServices on each endpoint class in external assemblies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="scanMarkers"></param>
        public static void AddMinimalApiDefinitionsForMarkers(this IServiceCollection services, IConfiguration configuration, params Type[] scanMarkers)
        {
            List<IEndpointDefinition> endpointDefinitions = new();

            foreach (var marker in scanMarkers)
            {
                endpointDefinitions.AddRange(marker.Assembly.ExportedTypes
                    .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Cast<IEndpointDefinition>());
            }

            foreach (var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.DefineServices(services, configuration);
            }

            services.AddSingleton<IReadOnlyCollection<IEndpointDefinition>>(endpointDefinitions);			
		}

        /// <summary>
        /// Extension method to execute DefineEndpoints on each endpoint class
        /// </summary>
        /// <param name="app"></param>
        public static void UseEndpointDefinitions(this WebApplication app)
        {
            var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

            foreach (var endpointDefinition in definitions)
            {
                endpointDefinition.DefineEndpoints(app);
            }
        }
    }
}
