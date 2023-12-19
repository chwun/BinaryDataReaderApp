namespace BinaryDataReaderApp.Models;

/// <summary>
/// Data class for loop settings of binary parts
/// </summary>
public class LoopSettings : ModelBase
{
	private int loopCountFixed;
	private long loopCountReference;
	private LoopType type;

	/// <summary>
	/// Loop type
	/// </summary>
	public LoopType Type
	{
		get => type;
		set
		{
			type = value;

			switch (type)
			{
				case LoopType.NONE:
					LoopCountReference = 0;
					LoopCountFixed = 0;
					break;

				case LoopType.FIXED:
					LoopCountReference = 0;
					break;

				case LoopType.REFERENCE:
					LoopCountFixed = 0;
					break;
			}

			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Fixed loop count
	/// </summary>
	public int LoopCountFixed
	{
		get => loopCountFixed;
		set
		{
			loopCountFixed = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// ID of binary part containing loop count
	/// </summary>
	public long LoopCountReference
	{
		get => loopCountReference;
		set
		{
			loopCountReference = value;
			OnPropertyChanged();
		}
	}
}