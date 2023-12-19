using System.Globalization;
using System.Windows.Data;

namespace BinaryDataReaderApp.Converters;

public class EnumToBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null || parameter == null)
		{
			return false;
		}

		string selectedValue = value.ToString();
		string checkValue = parameter.ToString();

		return selectedValue == checkValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null || parameter == null)
		{
			return null;
		}

		bool valueSelected = (bool)value;
		if (valueSelected)
		{
			string targetValue = parameter.ToString();

			return Enum.Parse(targetType, targetValue, true);
		}

		return null;
	}
}