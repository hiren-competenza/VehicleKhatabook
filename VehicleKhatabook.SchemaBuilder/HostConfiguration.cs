using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VehicleKhatabook.SchemaBuilder
{
    public class HostConfiguration
    {
        public IConfiguration Configuration { get; private set; }
        public ILoggerFactory LoggerFactory { get; private set; }

        /// <summary>
        /// Constructor that is evoked on build
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private HostConfiguration(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Build an instance of the host configuration
        /// </summary>
        /// <param name="args"></param>
        public static HostConfiguration Build(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
                builder =>
                {
                    var logConfigSection = configuration.GetSection("Logging");
                    builder.AddConfiguration(logConfigSection);
                    builder.AddSimpleConsole(
                        consoleConfig =>
                        {
                            consoleConfig.IncludeScopes = true;
                            consoleConfig.SingleLine = false;
                            consoleConfig.TimestampFormat = "MM-dd-yyyy HH:mm:ss ";
                            consoleConfig.UseUtcTimestamp = true;
                        });
                });

            var hostConfiguration = new HostConfiguration(configuration, loggerFactory);
            return hostConfiguration;
        }
    }
}
