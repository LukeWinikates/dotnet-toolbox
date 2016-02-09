using System;
using System.Threading;

namespace dotnet_toolbox.api.BackgroundWorker
{
    public interface ITimerProvider
    {
        Timer StartWithCallback(Action action);
    }
}