using System.Collections.ObjectModel;

namespace BinaryDataReaderApp.Models;

/// <summary>
/// Data class representing a section (e.g. struct) in a binary data template
/// </summary>
public class BinarySection : BinaryPart
{
	private LoopSettings loopSettings;
	private ObservableCollection<BinaryPart> parts;

	/// <summary>
	/// Creates new instance of BinarySection with ID and name
	/// </summary>
	/// <param name="id">ID of this value</param>
	/// <param name="name">Name of this value</param>
	public BinarySection(long id, string name)
		: base(id, name)
	{
		Parts = new();
	}

	/// <summary>
	/// Loop settings for this section
	/// </summary>
	public LoopSettings LoopSettings
	{
		get => loopSettings;
		set
		{
			loopSettings = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<BinaryPart> Parts
	{
		get => parts;
		set
		{
			parts = value;
			OnPropertyChanged();
		}
	}
}