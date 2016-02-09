using System;
using System.Threading;
using dotnet_toolbox.api.Env;
using Microsoft.Extensions.Logging;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public class RealTimerProvider : ITimerProvider
    {
        ILogger logger = LoggingConfiguration.CreateLogger<RealTimerProvider>();
        public Timer StartWithCallback(Action action)
        {
            var timer = new Timer(_ =>
            {
                logger.LogDebug("Timer triggered");
                action();
                logger.LogDebug("Timer callback completed");
            }, null, 1000, 2000);
            return timer;
        }
    }
}