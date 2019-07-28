using System.Collections.Generic;

namespace BinaryDataReaderApp.Models
{
    public class EnumStringConverter
    {
        private Dictionary<int, string> enumValues;

        public string Name { get; private set; }

        public EnumStringConverter(string name)
        {
            enumValues = new Dictionary<int, string>();
            Name = name;
        }

        public string GetText(int value)
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

        public void AddValue(int value, string text)
        {
            enumValues[value] = text;
        }
    }
}