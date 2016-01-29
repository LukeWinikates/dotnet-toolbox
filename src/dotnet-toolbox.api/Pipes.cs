using System;

namespace dotnet_toolbox.api
{
    public static class Pipes
    {
        public static TResult Pipe<TType, TResult>(this TType initial, Func<TType, TResult> func)
        {
            return func(initial);
        }
    }
}