using System;
using BinaryDataReaderApp.Localization;

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
				OnPropertyChanged(nameof(DescriptionText));
			}
		}

		public string DescriptionText
		{
			get
			{
				string text = TranslationManager.Instance.GetResourceText("BinaryFile_ByteOffset");
				text += ": ";
				text += ByteOffset;
				return text;
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
				if (value != isSelected)
				{
					isSelected = value;
					OnPropertyChanged();
				}
			}
		}

		public HexDumpByte(string text, int byteOffset)
		{
			Text = text;
			ByteOffset = byteOffset;
			IsSelected = false;
		}

		public static HexDumpByte UnusedByte = new HexDumpByte("--", 0);
	}
}