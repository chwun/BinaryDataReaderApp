using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BinaryDataReaderApp.Common;

namespace BinaryDataReaderApp.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private ObservableCollection<TabViewModelBase> tabVMs;

		public ObservableCollection<TabViewModelBase> TabVMs
		{
			get
			{
				return tabVMs;
			}
			set
			{
				tabVMs = value;
				OnPropertyChanged();
			}
		}

		#region Events

		public delegate void FileDialogEventRequestedHandler(object sender, FileDialogEventArgs e);
		public event FileDialogEventRequestedHandler FileDialogRequested;

		#endregion

		public MainViewModel()
		{
			TabVMs = new ObservableCollection<TabViewModelBase>();
		}

		#region Commands

		private ICommand newTemplateCommand;
		public ICommand NewTemplateCommand
		{
			get
			{
				if (newTemplateCommand == null)
				{
					newTemplateCommand = new ActionCommand(NewTemplateCommand_Executed, NewTemplateCommand_CanExecute);
				}

				return newTemplateCommand;
			}
		}

		private ICommand loadTemplateCommand;
		public ICommand LoadTemplateCommand
		{
			get
			{
				if (loadTemplateCommand == null)
				{
					loadTemplateCommand = new ActionCommand(LoadTemplateCommand_Executed, LoadTemplateCommand_CanExecute);
				}

				return loadTemplateCommand;
			}
		}
		public ICommand SaveTemplateCommand;
		#endregion

		#region Command-Handler
		private bool NewTemplateCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void NewTemplateCommand_Executed(object parameter)
		{
			TabVMs.Add(new BinaryTemplateTabViewModel("testabc"));
		}

		private bool LoadTemplateCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void LoadTemplateCommand_Executed(object parameter)
		{
			FileDialogEventArgs fileDialogEventArgs = new FileDialogEventArgs();
			FileDialogRequested?.Invoke(this, fileDialogEventArgs);

			if (!string.IsNullOrWhiteSpace(fileDialogEventArgs.File))
			{
				BinaryTemplateTabViewModel loadedTemplate = new BinaryTemplateTabViewModel("new template");

				if (loadedTemplate.LoadTemplateFromFile(fileDialogEventArgs.File))
				{
					TabVMs.Add(loadedTemplate);
				}
				else
				{
					// TODO!
				}
			}
		}

		#endregion
	}
}