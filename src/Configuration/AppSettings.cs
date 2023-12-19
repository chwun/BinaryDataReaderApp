using Microsoft.Extensions.Configuration;

namespace BinaryDataReaderApp.Configuration;

public class AppSettings
{
	public const string Key_ShowHexDump = "ShowHexDump";
	public const string Key_TemplateDirectory = "TemplateDirectory";

	private static AppSettings instance;

	private readonly IConfiguration configuration;

	public AppSettings()
	{
		configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
	}

	public static AppSettings Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new();
			}

			return instance;
		}
	}

	public bool GetConfigValue_Bool(string key) => bool.Parse(configuration[key]);

	public string GetConfigValue_String(string key) => configuration[key];
}