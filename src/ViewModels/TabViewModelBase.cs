namespace BinaryDataReaderApp.ViewModels;

public abstract class TabViewModelBase : ViewModelBase
{
	protected string header;

	protected TabViewModelBase(string header)
	{
		Header = header;
	}

	public string Header
	{
		get => header;
		set
		{
			header = value;
			OnPropertyChanged();
		}
	}
}