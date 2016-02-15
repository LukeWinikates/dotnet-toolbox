using System.Collections.Generic;
using dotnet_toolbox.api.Models;
using dotnet_toolbox.api.Query;
using StackExchange.Redis;

namespace dotnet_toolbox.api.VersionHistory
{
    public class VersionsList : IRedisHashable, IVersionsList
    {
        public List<Version> Versions { get; set; }

        public HashEntry[] AsRedisHash()
        {
           return new VersionsHashConverter(this).AsRedisHash();
        }

        public void FromRedisHash(HashEntry[] entries)
        {
            throw new System.NotImplementedException();
        }
    }
}