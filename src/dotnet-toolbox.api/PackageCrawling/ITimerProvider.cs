using System;
using System.Threading;

namespace dotnet_toolbox.api.PackageCrawling
{
    public interface ITimerProvider
    {
        Timer StartWithCallback(Action action);
    }
}