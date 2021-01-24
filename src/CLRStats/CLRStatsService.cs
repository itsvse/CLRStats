using System.Threading;

namespace CLRStats
{
    public class CLRStatsService
    {
        internal const string DEFAULT_PATH = "/clr";
        private readonly static CLRStatsModel cLRStatsModel = new CLRStatsModel();

        public static CLRStatsModel GetCurrentCLRStats()
        {
            return cLRStatsModel;
        }

        public static string GetCurrentCLRStatsToJson()
        {
            return GetCurrentCLRStats().ToJson();
        }
    }
}
