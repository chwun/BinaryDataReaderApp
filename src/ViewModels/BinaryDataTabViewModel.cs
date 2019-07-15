using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class BinaryDataTabViewModel : TabViewModelBase
	{
		private BinaryFile binaryFile;

		public BinaryFile BinaryFile
		{
			get
			{
				return binaryFile;
			}
			set
			{
				binaryFile = value;
				OnPropertyChanged();
			}
		}

		public BinaryDataTabViewModel(string header, string file, BinaryDataTemplate template)
		: base(header)
		{
			BinaryFile = new BinaryFile(file, template);
		}
	}
}