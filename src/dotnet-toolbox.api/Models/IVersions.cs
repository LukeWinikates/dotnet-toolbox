using System.Collections.Generic;
using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.Models
{
    public interface IVersionsList : IRedisHashable
    {
        List<Version> Versions {get;set;}
    }
}