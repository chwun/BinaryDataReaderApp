using System;
using System.Linq;
using System.Text;

namespace BinaryDataReaderApp.Models
{
	public class IntAsciiConverter : IntToStringConverter
	{
		public IntAsciiConverter(string name)
		{
			Name = name;
		}

		public override void AddMapping(int value, string text)
		{
		}

		public override string GetText(int value)
		{
			if (value > 255)
			{
				throw new ArgumentException($"{value} is no valid ASCII value!");
			}

			byte[] asciiByte = BitConverter.GetBytes(value).Take(1).ToArray();

			return Encoding.ASCII.GetString(asciiByte);
		}
	}
}