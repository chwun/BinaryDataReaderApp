using BinaryDataReaderApp.Configuration;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels;

public class BinaryDataTabViewModel : TabViewModelBase
{
	private BinaryFile binaryFile;
	private string binaryFilePath;
	private BinaryPart selectedPart;
	private bool showHexDump;
	private BinaryDataTemplate template;

	public BinaryDataTabViewModel(string header, string binaryFilePath, BinaryDataTemplate template)
		: base(header)
	{
		this.template = template;

		BinaryFilePath = binaryFilePath;

		ShowHexDump = AppSettings.Instance.GetConfigValue_Bool(AppSettings.Key_ShowHexDump);

		if (template != null)
		{
			BinaryFile = new(binaryFilePath, template);
			BinaryFile.Read();
		}
		// TODO!
	}

	public BinaryFile BinaryFile
	{
		get => binaryFile;
		set
		{
			binaryFile = value;
			OnPropertyChanged();
		}
	}

	public string BinaryFilePath
	{
		get => binaryFilePath;
		set
		{
			binaryFilePath = value;
			OnPropertyChanged();
		}
	}

	public BinaryPart SelectedPart
	{
		get => selectedPart;
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
		get => showHexDump;
		set
		{
			showHexDump = value;
			OnPropertyChanged();
		}
	}

	public void SetSelectionInTree(object selectedHexDumpRowItem, int selectedHexDumpColumnIndex)
	{
		// TODO: Auswahl im Tree funktioniert noch nicht!

		if (selectedHexDumpRowItem is HexDumpLine selectedHexDumpLine)
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