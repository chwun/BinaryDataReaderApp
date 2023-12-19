namespace BinaryDataReaderApp.Models;

public static class BinaryValueTypeExtensions
{
	public static int GetByteSize(this BinaryValue value)
	{
		return value.ValueType switch
		{
			BinaryValueType.BYTE => 1,
			BinaryValueType.BOOL => 1,
			BinaryValueType.SHORT => 2,
			BinaryValueType.USHORT => 2,
			BinaryValueType.INT => 4,
			BinaryValueType.UINT => 4,
			BinaryValueType.FLOAT => 4,
			BinaryValueType.LONG => 8,
			BinaryValueType.ULONG => 8,
			BinaryValueType.DOUBLE => 8,
			BinaryValueType.STRING => value.Length,
			BinaryValueType.BLOB => value.Length,
			_ => throw new ArgumentException("invalid value type")
		};
	}

	public static string GetValueText(this BinaryValue value)
	{
		if (value.ValueType == BinaryValueType.STRING)
		{
			return $"\"{value.Value}\"";
		}

		if (value.ValueType == BinaryValueType.BLOB)
		{
			return string.Join("", (value.Value as byte[])!.Select(x => $"{x:X2}"));
		}

		if (value.Converter != null)
		{
			try
			{
				string convertedText = value.Converter.GetText(Convert.ToInt32(value.Value));
				return $"{convertedText} ({value.Value})";
			}
			catch
			{
			}
		}

		return value.Value.ToString();
	}
}