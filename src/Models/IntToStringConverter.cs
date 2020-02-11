namespace BinaryDataReaderApp.Models
{
	public abstract class IntToStringConverter
	{
		public string Name { get; protected set; }

		public abstract void AddMapping(int value, string text);
		
		public abstract string GetText(int value);
	}
}