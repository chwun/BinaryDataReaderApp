using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Localization;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class TemplatePartDetailsWindowViewModel : ViewModelBase
	{
		public TemplatePartDetailsWindowEventArgs DetailEventArgs { get; private set; }

		private string dialogTitle;
		private string partName;
		private BinaryValueType valueType;
		private LoopSettings loopSettings;

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

		#endregion

		public TemplatePartDetailsWindowViewModel(TemplatePartDetailsWindowEventArgs e)
		{
			DetailEventArgs = e;

			if (e.Part is BinarySection section)
			{
				Section = section;
				DialogTitle = TranslationManager.Instance.GetResourceText("BinaryTemplateProperties_DialogTitle_Section");

				PartName = Section.Name;
				LoopSettings = new LoopSettings()
				{
					Type = section.LoopSettings.Type,
					LoopCountFixed = section.LoopSettings.LoopCountFixed,
					LoopCountReference = section.LoopSettings.LoopCountReference
				};
			}
			else if (e.Part is BinaryValue value)
			{
				Value = value;
				DialogTitle = TranslationManager.Instance.GetResourceText("BinaryTemplateProperties_DialogTitle_Value");

				PartName = Value.Name;
				ValueType = Value.ValueType;
			}
		}

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

		#endregion

		#region command handlers

		private bool OkCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void OkCommand_Executed(object parameter)
		{
			DetailEventArgs.DialogResult = true;

			DetailEventArgs.PartName = PartName;

			if (Section != null)
			{
				DetailEventArgs.LoopSettings = new LoopSettings()
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

		#endregion

		#region private methods

		#endregion
	}
}