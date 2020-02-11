using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class BinaryTemplateTabViewModel : TabViewModelBase
	{
		private BinaryDataTemplate binaryTemplate;
		private BinaryPart selectedPart;

		public BinaryDataTemplate BinaryTemplate
		{
			get
			{
				return binaryTemplate;
			}
			set
			{
				binaryTemplate = value;
				OnPropertyChanged();
			}
		}

		public BinaryPart SelectedPart
		{
			get
			{
				return selectedPart;
			}
			set
			{
				selectedPart = value;
				OnPropertyChanged();
			}
		}

		public BinaryTemplateTabViewModel(string header) : base(header)
		{
		}

		public bool LoadTemplateFromFile(string file)
		{
			BinaryTemplate = new BinaryDataTemplate(Header);

			if (BinaryTemplate.ReadFromXML(new XMLAccess(file)))
			{
				Header = BinaryTemplate.Name;
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool SaveTemplateToFile(string file)
		{
			return BinaryTemplate.SaveToXML(new XMLAccess(file));
		}
	}
}