namespace BinaryDataReaderApp.Models;

public class HexDumpLine : ModelBase
{
	private readonly List<HexDumpByte> hexBytes;
	private int byteOffset;

	public HexDumpLine(int byteOffset)
	{
		ByteOffset = byteOffset;
		HexBytes = new();
	}

	public int ByteOffset
	{
		get => byteOffset;
		private set
		{
			byteOffset = value;
			OnPropertyChanged();
		}
	}

	public List<HexDumpByte> HexBytes
	{
		get => hexBytes;
		private init
		{
			hexBytes = value;
			OnPropertyChanged();
		}
	}

	public HexDumpByte this[int i]
	{
		get
		{
			if (HexBytes.Count > i)
			{
				return HexBytes[i];
			}

			return HexDumpByte.UnusedByte;
		}
	}
}