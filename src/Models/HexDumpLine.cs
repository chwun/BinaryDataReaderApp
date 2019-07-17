using System.Collections.Generic;

namespace BinaryDataReaderApp.Models
{
	public class HexDumpLine : ModelBase
	{
		private int byteOffset;
		private List<HexDumpByte> hexBytes;

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

		public List<HexDumpByte> HexBytes
		{
			get
			{
				return hexBytes;
			}
			private set
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
				else
				{
					return HexDumpByte.UnusedByte;
				}
			}
		}

		public HexDumpLine(int byteOffset)
		{
			ByteOffset = byteOffset;
			HexBytes = new List<HexDumpByte>();
		}
	}
}