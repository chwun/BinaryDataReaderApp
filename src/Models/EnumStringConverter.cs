using System.Collections.Generic;
using System.Linq;

namespace BinaryDataReaderApp.Models
{

	public class EnumToStringConverter : IntToStringConverter
	{
		private Dictionary<int, string> enumValues;

		public EnumToStringConverter(string name)
		{
			enumValues = new Dictionary<int, string>();
			Name = name;
		}

		public override string GetText(int value)
		{
			if (enumValues.ContainsKey(value))
			{
				return enumValues[value];
			}
			else
			{
				return "";
			}
		}

		public override void AddMapping(int value, string text)
		{
			enumValues[value] = text;
		}

		public IEnumerable<KeyValuePair<int, string>> GetMappings()
		{
			return enumValues.Select(x => x);
		}
	}
}