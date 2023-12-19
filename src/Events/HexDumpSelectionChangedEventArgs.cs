using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.Events;

public class HexDumpSelectionChangedEventArgs : EventArgs
{
	public HexDumpLine SelectedHexDumpLine;
}