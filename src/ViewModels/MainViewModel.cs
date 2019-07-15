using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private ObservableCollection<TabViewModelBase> tabVMs;
		private int selectedTabIndex;

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

		public int SelectedTabIndex
		{
			get
			{
				return selectedTabIndex;
			}
			set
			{
				selectedTabIndex = value;
				OnPropertyChanged();
			}
		}

		#region events

		public delegate void FileDialogRequestedEventHandler(object sender, FileDialogEventArgs e);
		public delegate void CloseRequestedEventHandler(object sender, EventArgs e);

		public event FileDialogRequestedEventHandler FileDialogRequested;
		public event CloseRequestedEventHandler CloseRequested;

		#endregion

		public MainViewModel()
		{
			TabVMs = new ObservableCollection<TabViewModelBase>();
		}

		#region commands

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

		private ICommand openBinaryCommand;
		public ICommand OpenBinaryCommand
		{
			get
			{
				if (openBinaryCommand == null)
				{
					openBinaryCommand = new ActionCommand(OpenBinaryCommand_Executed, OpenBinaryCommand_CanExecute);
				}

				return openBinaryCommand;
			}
		}

		private ICommand closeTabCommand;
		public ICommand CloseTabCommand
		{
			get
			{
				if (closeTabCommand == null)
				{
					closeTabCommand = new ActionCommand(CloseTabCommand_Executed, CloseTabCommand_CanExecute);
				}

				return closeTabCommand;
			}
		}

		private ICommand closeCommand;
		public ICommand CloseCommand
		{
			get
			{
				if (closeCommand == null)
				{
					closeCommand = new ActionCommand(CloseCommand_Executed, CloseCommand_CanExecute);
				}

				return closeCommand;
			}
		}


		#endregion

		#region command handlers
		private bool NewTemplateCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void NewTemplateCommand_Executed(object parameter)
		{
			var newTab = new BinaryTemplateTabViewModel("new template");
			AddNewTab(newTab);
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
				BinaryTemplateTabViewModel loadedTemplate = new BinaryTemplateTabViewModel("loaded template");

				if (loadedTemplate.LoadTemplateFromFile(fileDialogEventArgs.File))
				{
					AddNewTab(loadedTemplate);
				}
				else
				{
					// TODO!
				}
			}
		}

		private bool OpenBinaryCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void OpenBinaryCommand_Executed(object parameter)
		{
			string file = "";
			BinaryDataTemplate template = null;
			var newTab = new BinaryDataTabViewModel("binary file", file, template);
			AddNewTab(newTab);
		}

		private bool CloseTabCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void CloseTabCommand_Executed(object parameter)
		{
			CloseTab(parameter as TabViewModelBase);
		}

		private bool CloseCommand_CanExecute(object parameter)
		{
			return true;
		}

		private void CloseCommand_Executed(object parameter)
		{
			CloseRequested?.Invoke(this, new EventArgs());
		}

		#endregion

		#region private methods

		private void AddNewTab(TabViewModelBase tab)
		{
			TabVMs.Add(tab);
			SelectedTabIndex = TabVMs.Count - 1;
		}

		private void CloseTab(TabViewModelBase tab)
		{
			if (tab != null)
			{
				TabVMs.Remove(tab);
			}
		}

		#endregion
	}
}