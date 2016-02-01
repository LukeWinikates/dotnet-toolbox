using System.Collections.Generic;

namespace dotnet_toolbox.api.Query {
    public interface ILatestPackagesIndex
    {
        IEnumerable<string> Get();
        void Update(long timestamp, string packageName);
    }
}