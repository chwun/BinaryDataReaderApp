using Microsoft.Extensions.Configuration;

namespace BinaryDataReaderApp.Configuration
{
    public class AppSettings
    {
        public const string Key_ShowHexDump = "ShowHexDump";
        public const string Key_TemplateDirectory = "TemplateDirectory";

        private static AppSettings instance;

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppSettings();
                }

                return instance;
            }
        }

        private IConfiguration configuration;

        public AppSettings()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
        }

        public bool GetConfigValue_Bool(string key)
        {
            return bool.Parse(configuration[key]);
        }

        public string GetConfigValue_String(string key)
        {
            return configuration[key];
        }
    }
}