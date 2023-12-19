using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Localization;
using BinaryDataReaderApp.Models;
using System.Windows.Input;

namespace BinaryDataReaderApp.ViewModels;

public class TemplatePartDetailsWindowViewModel : ViewModelBase
{
	private string dialogTitle;
	private LoopSettings loopSettings;
	private string partName;
	private BinaryValueType valueType;

	public TemplatePartDetailsWindowViewModel(TemplatePartDetailsWindowEventArgs e)
	{
		DetailEventArgs = e;

		if (e.Part is BinarySection section)
		{
			Section = section;
			DialogTitle = TranslationManager.GetResourceText("BinaryTemplateProperties_DialogTitle_Section");

			PartName = Section.Name;
			LoopSettings = new()
			{
				Type = section.LoopSettings.Type,
				LoopCountFixed = section.LoopSettings.LoopCountFixed,
				LoopCountReference = section.LoopSettings.LoopCountReference
			};
		}
		else if (e.Part is BinaryValue value)
		{
			Value = value;
			DialogTitle = TranslationManager.GetResourceText("BinaryTemplateProperties_DialogTitle_Value");

			PartName = Value.Name;
			ValueType = Value.ValueType;
		}
	}

	public TemplatePartDetailsWindowEventArgs DetailEventArgs { get; }

	public string DialogTitle
	{
		get => dialogTitle;
		set
		{
			dialogTitle = value;
			OnPropertyChanged();
		}
	}

	public string PartName
	{
		get => partName;
		set
		{
			partName = value;
			OnPropertyChanged();
		}
	}

	public BinaryValueType ValueType
	{
		get => valueType;
		set
		{
			valueType = value;
			OnPropertyChanged();
		}
	}

	public LoopSettings LoopSettings
	{
		get => loopSettings;
		set
		{
			loopSettings = value;

			OnPropertyChanged();
		}
	}

	public BinarySection Section { get; }

	public BinaryValue Value { get; }

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
		DetailEventArgs.DialogResult = true;

		DetailEventArgs.PartName = PartName;

		if (Section != null)
		{
			DetailEventArgs.LoopSettings = new()
			{
				Type = LoopSettings.Type,
				LoopCountFixed = LoopSettings.LoopCountFixed,
				LoopCountReference = LoopSettings.LoopCountReference
			};
		}
		else if (Value != null)
		{
			DetailEventArgs.ValueType = ValueType;
		}

		CloseRequested?.Invoke(this, null);
	}

	#endregion command handlers

	#region private methods

	#endregion private methods
}