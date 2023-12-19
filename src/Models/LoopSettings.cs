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

			if (type == LoopType.NONE)
			{
				LoopCountReference = 0;
				LoopCountFixed = 0;
			}
			else if (type == LoopType.FIXED)
			{
				LoopCountReference = 0;
			}
			else if (type == LoopType.REFERENCE)
			{
				LoopCountFixed = 0;
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