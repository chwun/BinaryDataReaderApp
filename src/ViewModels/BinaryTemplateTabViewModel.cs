using System;
using System.Windows.Input;
using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Events;
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

		#region events

		public delegate void TemplatePartDetailsWindowHandler(object sender, TemplatePartDetailsWindowEventArgs e);

		public event TemplatePartDetailsWindowHandler TemplatePartDetailsWindowRequested;

		#endregion

		public BinaryTemplateTabViewModel(string header) : base(header)
		{
		}

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


		#endregion

		#region command handlers

		private bool ShowDetailsCommand_CanExecute(object parameter)
		{
			return ((parameter as BinaryPart) != null);
		}

		private void ShowDetailsCommand_Executed(object parameter)
		{
			BinaryPart part = parameter as BinaryPart;

			TemplatePartDetailsWindowEventArgs detailsWindowEventArgs = new TemplatePartDetailsWindowEventArgs()
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

		#endregion

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