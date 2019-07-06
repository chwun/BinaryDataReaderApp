using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class BinaryTemplateTabViewModel : TabViewModelBase
	{
		private BinaryDataTemplate binaryTemplate;

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

		public BinaryTemplateTabViewModel(string header) : base(header)
		{
		}

		public bool LoadTemplateFromFile(string file)
		{
			BinaryTemplate = new BinaryDataTemplate(Header);

			if (BinaryTemplate.ReadFromXML(new BinaryDataTemplateXMLProvider(file)))
			{
				Header = BinaryTemplate.Name;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}