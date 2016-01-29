using Microsoft.Extensions.Logging;

namespace dotnet_toolbox.api.Env
{
    public static class LoggingConfiguration
    {
        static LoggerFactory factory = new LoggerFactory();

        public static void StartConsoleLogging() {
            factory.AddConsole();
        }

        internal static Microsoft.Extensions.Logging.ILogger CreateLogger<T>()
        {
            return factory.CreateLogger<T>();
        }
    }
}