using System.Globalization;
using System.Windows;

namespace BinaryDataReaderApp.Localization;

public class TranslationManager
{
	public static void SetLanguage(CultureInfo ci)
	{
		Thread.CurrentThread.CurrentCulture = ci;
		Thread.CurrentThread.CurrentUICulture = ci;

		ResourceDictionary dict = new()
		{
			Source = new($"pack://application:,,,/Localization/{ci.Name}.xaml")
		};

		ResourceDictionary existingDict =
			Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd =>
				rd.Source.OriginalString.StartsWith("pack://application:,,,/Localization/"));

		if (existingDict != null)
		{
			Application.Current.Resources.MergedDictionaries.Remove(existingDict);
		}

		Application.Current.Resources.MergedDictionaries.Add(dict);
	}

	public static string GetResourceText(string key)
	{
		ResourceDictionary dict =
			Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd =>
				rd.Source.OriginalString.StartsWith("pack://application:,,,/Localization/"));
		return dict[key] as string;
	}
}