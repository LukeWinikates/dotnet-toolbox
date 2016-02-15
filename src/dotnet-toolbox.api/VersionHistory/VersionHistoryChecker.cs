using dotnet_toolbox.api.Query;

namespace dotnet_toolbox.api.VersionHistory
{
    public class VersionHistoryChecker
    {
        private IGetSetQuerier<VersionsList> querier;
        private IVersionHistoryCheck versionCheck;
        public VersionHistoryChecker(IGetSetQuerier<VersionsList> querier, IVersionHistoryCheck versionCheck)
        {
            this.querier = querier;
            this.versionCheck = versionCheck;
        }

        public void LoadVersionHistory(string packageName)
        {
            versionCheck.Download(packageName)
                .DoTo(version => querier.Set(packageName, version));
        }
    }
}