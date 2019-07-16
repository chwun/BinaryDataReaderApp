namespace BinaryDataReaderApp.Models
{
	public class HexDumpByte : ModelBase
	{
		private string text;
		private int byteOffset;
		private bool isSelected;

		public string Text
		{
			get
			{
				return text;
			}
			private set
			{
				text = value;
				OnPropertyChanged();
			}
		}

		public int ByteOffset
		{
			get
			{
				return byteOffset;
			}
			private set
			{
				byteOffset = value;
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
			set
			{
				isSelected = value;
				OnPropertyChanged();
			}
		}

		public HexDumpByte(string text, int byteOffset)
		{
			Text = text;
			ByteOffset = byteOffset;
			IsSelected = false;
		}
	}
}