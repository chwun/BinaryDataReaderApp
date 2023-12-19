using BinaryDataReaderApp.Models;
using System.Globalization;
using System.Windows.Data;

namespace BinaryDataReaderApp.Converters;

public class BinaryValueTypeToBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is BinaryValue;

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}