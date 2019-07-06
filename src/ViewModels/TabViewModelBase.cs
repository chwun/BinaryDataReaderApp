namespace BinaryDataReaderApp.ViewModels
{
	public abstract class TabViewModelBase : ViewModelBase
	{
		protected string header;

		public string Header
		{
			get
			{
				return header;
			}
			set
			{
				header = value;
				OnPropertyChanged();
			}
		}

		public TabViewModelBase(string header)
		{
			Header = header;
		}
	}
}