using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Models;
using System.Windows.Input;

namespace BinaryDataReaderApp.ViewModels;

public class BinaryTemplateTabViewModel : TabViewModelBase
{
	private BinaryDataTemplate binaryTemplate;
	private BinaryPart selectedPart;

	public BinaryTemplateTabViewModel(string header) : base(header)
	{
	}

	public BinaryDataTemplate BinaryTemplate
	{
		get => binaryTemplate;
		set
		{
			binaryTemplate = value;
			OnPropertyChanged();
		}
	}

	public BinaryPart SelectedPart
	{
		get => selectedPart;
		set
		{
			selectedPart = value;
			OnPropertyChanged();
		}
	}

	public bool LoadTemplateFromFile(string file)
	{
		BinaryTemplate = new(Header);

		if (BinaryTemplate.ReadFromXML(new XMLAccess(file)))
		{
			Header = BinaryTemplate.Name;
			return true;
		}

		return false;
	}

	public bool SaveTemplateToFile(string file) => BinaryTemplate.SaveToXML(new XMLAccess(file));

	#region events

	public delegate void TemplatePartDetailsWindowHandler(object sender, TemplatePartDetailsWindowEventArgs e);

	public event TemplatePartDetailsWindowHandler TemplatePartDetailsWindowRequested;

	#endregion events

	#region commands

	private ICommand showDetailsCommand;

	public ICommand ShowDetailsCommand
	{
		get
		{
			if (showDetailsCommand == null)
			{
				showDetailsCommand = new ActionCommand(ShowDetailsCommand_Executed, ShowDetailsCommand_CanExecute);
			}

			return showDetailsCommand;
		}
	}

	#endregion commands

	#region command handlers

	private bool ShowDetailsCommand_CanExecute(object parameter) => parameter as BinaryPart != null;

	private void ShowDetailsCommand_Executed(object parameter)
	{
		BinaryPart part = parameter as BinaryPart;

		TemplatePartDetailsWindowEventArgs detailsWindowEventArgs = new()
		{
			Part = part
		};

		TemplatePartDetailsWindowRequested?.Invoke(this, detailsWindowEventArgs);

		if (detailsWindowEventArgs.DialogResult)
		{
			part.Name = detailsWindowEventArgs.PartName;

			if (part is BinarySection section)
			{
				section.LoopSettings.Type = detailsWindowEventArgs.LoopSettings.Type;
				section.LoopSettings.LoopCountFixed = detailsWindowEventArgs.LoopSettings.LoopCountFixed;
				section.LoopSettings.LoopCountReference = detailsWindowEventArgs.LoopSettings.LoopCountReference;
			}
			else if (part is BinaryValue value)
			{
				value.ValueType = detailsWindowEventArgs.ValueType;
			}
		}
	}

	#endregion command handlers
}