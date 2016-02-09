using System.Linq;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Query
{
    public static class HashEntryExtensions
    {
        public static RedisValue ValueFor(this HashEntry[] entries, string name) => entries.FirstOrDefault(e => e.Name == name).Value;
        public static int? IntValueFor(this HashEntry[] entries, string name) => !entries.ValueFor(name).IsNullOrEmpty ? (int?)int.Parse(entries.ValueFor(name)) : null;
    }
}