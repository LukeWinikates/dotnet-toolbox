using System;

public interface ITimerProvider
{
    void StartWithCallback(Action action);
}