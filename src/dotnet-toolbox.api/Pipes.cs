using System;

namespace dotnet_toolbox.api
{
    public static class Pipes
    {
        public static TResult Pipe<TType, TResult>(this TType initial, Func<TType, TResult> func)
        {
            return func(initial);
        }
        
        public static TType DoTo<TType>(this TType initial, params Action<TType>[] actions) {
            foreach(var action in actions) {
                action(initial);
            }
            return initial;
        }
    }
}