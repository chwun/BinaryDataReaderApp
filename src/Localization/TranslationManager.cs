using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace BinaryDataReaderApp.Localization
{
    public class TranslationManager
    {
        private static TranslationManager instance = new TranslationManager();

		public static TranslationManager Instance
		{
			get
			{
				return instance;
			}
		}

		public void SetLanguage(CultureInfo ci)
		{
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			var dict = new ResourceDictionary
			{
				Source = new Uri(string.Format("pack://application:,,,/Localization/{0}.xaml", ci.Name))
			};

			var existingDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source.OriginalString.StartsWith("pack://application:,,,/Localization/"));

			if (existingDict != null)
			{
				Application.Current.Resources.MergedDictionaries.Remove(existingDict);
			}

			Application.Current.Resources.MergedDictionaries.Add(dict);
		}

		public string GetResourceText(string key)
		{
			var dict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source.OriginalString.StartsWith("pack://application:,,,/Localization/"));
			return dict[key] as string;
		}
    }
}