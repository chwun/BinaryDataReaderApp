using System;
using System.Linq;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class BinaryDataTabViewModel : TabViewModelBase
	{
		private BinaryFile binaryFile;
		private string binaryFilePath;
		private BinaryDataTemplate template;
		private BinaryPart selectedPart;

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
					// SetSelectionInHexDump(selectedPart);
				}
			}
		}

		public BinaryDataTabViewModel(string header, string binaryFilePath, string templateFile)
		: base(header)
		{
			BinaryDataTemplateXMLProvider templateProvider = new BinaryDataTemplateXMLProvider(templateFile);
			template = new BinaryDataTemplate("template");

			this.BinaryFilePath = binaryFilePath;

			if (template.ReadFromXML(templateProvider))
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