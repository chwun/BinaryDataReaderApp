using System;
using System.Globalization;
using System.Windows.Data;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp .Converters
{
	public class BinaryValueTypeToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value is BinaryValue);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}