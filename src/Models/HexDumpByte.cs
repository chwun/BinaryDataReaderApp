using BinaryDataReaderApp.Localization;

namespace BinaryDataReaderApp.Models;

public class HexDumpByte : ModelBase
{
	public static HexDumpByte UnusedByte = new("--", 0);
	private int byteOffset;
	private bool isSelected;
	private string text;

	public HexDumpByte(string text, int byteOffset)
	{
		Text = text;
		ByteOffset = byteOffset;
		IsSelected = false;
	}

	public string Text
	{
		get => text;
		private set
		{
			text = value;
			OnPropertyChanged();
		}
	}

	public int ByteOffset
	{
		get => byteOffset;
		private init
		{
			byteOffset = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(DescriptionText));
		}
	}

	public string DescriptionText
	{
		get
		{
			string text = TranslationManager.GetResourceText("BinaryFile_ByteOffset");
			text += ": ";
			text += ByteOffset;
			return text;
		}
	}

	public bool IsSelected
	{
		get => isSelected;
		set
		{
			if (value != isSelected)
			{
				isSelected = value;
				OnPropertyChanged();
			}
		}
	}
}