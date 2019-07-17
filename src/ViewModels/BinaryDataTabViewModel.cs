using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class BinaryDataTabViewModel : TabViewModelBase
	{
		private BinaryFile binaryFile;
		private string binaryFilePath;
		private BinaryDataTemplate template;
		private BinaryPart selectedPart;

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

		public string BinaryFilePath
		{
			get
			{
				return binaryFilePath;
			}
			set
			{
				binaryFilePath = value;
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
				if (value != selectedPart)
				{
					selectedPart = value;
					OnPropertyChanged();
				}
			}
		}

		public BinaryDataTabViewModel(string header, string binaryFilePath, string templateFile)
		: base(header)
		{
			BinaryDataTemplateXMLProvider templateProvider = new BinaryDataTemplateXMLProvider(templateFile);
			template = new BinaryDataTemplate("template");

			this.BinaryFilePath = binaryFilePath;

			if (template.ReadFromXML(templateProvider))
			{
				BinaryFile = new BinaryFile(binaryFilePath, template);
				BinaryFile.Read();
			}
			else
			{
				// TODO!
			}
		}

		private void SetSelectionInHexDump(BinaryPart part)
		{
			if (part is BinaryValue value)
			{
				// TODO
			}
		}
	}
}