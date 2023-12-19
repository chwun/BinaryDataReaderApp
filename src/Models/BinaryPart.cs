namespace BinaryDataReaderApp.Models;

/// <summary>
/// Abstract base class for a part in a binary data template
/// </summary>
public abstract class BinaryPart : ModelBase
{
	private long id;
	private string name;

	/// <summary>
	/// Creates new instance of BinaryPart with ID and name
	/// </summary>
	/// <param name="id">ID of this part</param>
	/// <param name="name">Name of this part</param>
	protected BinaryPart(long id, string name)
	{
		ID = id;
		Name = name;
	}

	protected BinaryPart()
		: this(-1, "")
	{
	}

	/// <summary>
	/// ID of this part
	/// </summary>
	public long ID
	{
		get => id;
		set
		{
			id = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Name of this part
	/// </summary>
	public string Name
	{
		get => name;
		set
		{
			name = value;
			OnPropertyChanged();
		}
	}
}