using BinaryDataReaderApp.Common;
using System.Windows.Input;

namespace BinaryDataReaderApp.ViewModels;

public class OpenBinaryFileWindowViewModel : ViewModelBase
{
	private string binaryFilePath;
	private string templatePath;

	public string TemplatePath
	{
		get => templatePath;
		set
		{
			templatePath = value;
			OnPropertyChanged();
		}
	}

	public string BinaryFilePath
	{
		get => binaryFilePath;
		set
		{
			binaryFilePath = value;
			OnPropertyChanged();
		}
	}

	public bool DialogResult { get; private set; }

	#region events

	public delegate void CloseRequestedEventHandler(object sender, EventArgs e);

	public event CloseRequestedEventHandler CloseRequested;

	#endregion events

	#region commands

	private ICommand okCommand;

	public ICommand OkCommand
	{
		get
		{
			if (okCommand == null)
			{
				okCommand = new ActionCommand(OkCommand_Executed, OkCommand_CanExecute);
			}

			return okCommand;
		}
	}

	#endregion commands

	#region command handlers

	private bool OkCommand_CanExecute(object parameter) => true;

	private void OkCommand_Executed(object parameter)
	{
		DialogResult = true;
		CloseRequested?.Invoke(this, null);
	}

	#endregion command handlers
}