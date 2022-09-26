using Services.Abstraction;

namespace Services.Core.App
{
    public class AppService : IAppService
    {
#if UNITY_EDITOR
        public const string SERVER_URL = "https://www.LUC.com/";
#else
        public const string SERVER_URL = "/";
#endif

        public string ServerURL => SERVER_URL;
    }
}