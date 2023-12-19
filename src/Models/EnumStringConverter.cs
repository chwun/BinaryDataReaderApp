namespace BinaryDataReaderApp.Models;

public class EnumToStringConverter : IntToStringConverter
{
	private readonly Dictionary<int, string> enumValues;

	public EnumToStringConverter(string name)
	{
		enumValues = new();
		Name = name;
	}

	public override string GetText(int value) => enumValues.TryGetValue(value, out string text) ? text : "";

	public override void AddMapping(int value, string text)
	{
		enumValues[value] = text;
	}

	public IEnumerable<KeyValuePair<int, string>> GetMappings()
	{
		return enumValues.Select(x => x);
	}
}