namespace BinaryDataReaderApp
{
	public abstract class TabItem
	{
		public string Header { get; set; }

		public object Data { get; set; }

		public TabItem(string header, object data)
		{
			Header = header;
			Data = data;
		}
	}
}