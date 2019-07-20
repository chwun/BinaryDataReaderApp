using System;
using System.Linq;
using BinaryDataReaderApp.Configuration;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class BinaryDataTabViewModel : TabViewModelBase
	{
		private BinaryFile binaryFile;
		private string binaryFilePath;
		private BinaryDataTemplate template;
		private BinaryPart selectedPart;
		private bool showHexDump;

		public BinaryFile BinaryFile
		{
			get
			{
				return binaryFile;
			}
			set
			{
				binaryFile = value;
				OnPropertyChanged();
			}
		}

		public string BinaryFilePath
		{
			get
			{
				return binaryFilePath;
			}
			set
			{
				binaryFilePath = value;
				OnPropertyChanged();
			}
		}

		public BinaryPart SelectedPart
		{
			get
			{
				return selectedPart;
			}
			set
			{
				if (value != selectedPart)
				{
					selectedPart = value;
					OnPropertyChanged();
					SetSelectionInHexDump(selectedPart);
				}
			}
		}

		public bool ShowHexDump
		{
			get{
				return showHexDump;
			}
			set{
				showHexDump = value;
				OnPropertyChanged();
			}
		}

		public BinaryDataTabViewModel(string header, string binaryFilePath, BinaryDataTemplate template)
		: base(header)
		{
			this.template = template;

			this.BinaryFilePath = binaryFilePath;

			ShowHexDump = AppSettings.Instance.GetConfigValue_Bool(AppSettings.Key_ShowHexDump);

			if (template != null)
			{
				BinaryFile = new BinaryFile(binaryFilePath, template);
				BinaryFile.Read();
			}
			else
			{
				// TODO!
			}
		}

		public void SetSelectionInTree(object selectedHexDumpRowItem, int selectedHexDumpColumnIndex)
		{
			// TODO: Auswahl im Tree funktioniert noch nicht!

			HexDumpLine selectedHexDumpLine = selectedHexDumpRowItem as HexDumpLine;
			if (selectedHexDumpLine != null)
			{
				int byteOffset = selectedHexDumpLine.ByteOffset;
				byteOffset += Math.Max(selectedHexDumpColumnIndex - 1, 0);
				SelectedPart = BinaryFile.FindValueByByteOffset(byteOffset);
			}
		}

		private void SetSelectionInHexDump(BinaryPart part)
		{
			if (part is BinaryValue value)
			{
				BinaryFile.SetSelectionInHexDump(value);
			}
		}
	}
}