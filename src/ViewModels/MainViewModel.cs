using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Configuration;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Localization;
using BinaryDataReaderApp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace BinaryDataReaderApp.ViewModels;

public class MainViewModel : ViewModelBase
{
	private readonly BinaryDataTemplateManager templateManager;
	private int selectedTabIndex;
	private ObservableCollection<TabViewModelBase> tabVMs;

	public MainViewModel()
	{
		TabVMs = new();
		templateManager = new(AppSettings.Instance.GetConfigValue_String(AppSettings.Key_TemplateDirectory));
	}

	public ObservableCollection<TabViewModelBase> TabVMs
	{
		get => tabVMs;
		set
		{
			tabVMs = value;
			OnPropertyChanged();
		}
	}

	public int SelectedTabIndex
	{
		get => selectedTabIndex;
		set
		{
			selectedTabIndex = value;
			OnPropertyChanged();
		}
	}

	#region events

	public delegate void FileDialogRequestedEventHandler(object sender, FileDialogEventArgs e);

	public delegate void CloseRequestedEventHandler(object sender, EventArgs e);

	public delegate void OpenBinaryFileWindowRequestedEventHandler(object sender, OpenBinaryFileWindowEventArgs e);

	public event FileDialogRequestedEventHandler FileDialogRequested;

	public event CloseRequestedEventHandler CloseRequested;

	// public event OpenBinaryFileWindowRequestedEventHandler OpenBinaryFileWindowRequested;

	#endregion events

	#region commands

	private ICommand newTemplateCommand;

	public ICommand NewTemplateCommand => newTemplateCommand ??= new ActionCommand(NewTemplateCommand_Executed, NewTemplateCommand_CanExecute);

	private ICommand loadTemplateCommand;

	public ICommand LoadTemplateCommand => loadTemplateCommand ??= new ActionCommand(LoadTemplateCommand_Executed, LoadTemplateCommand_CanExecute);

	private ICommand saveTemplateCommand;

	public ICommand SaveTemplateCommand => saveTemplateCommand ??= new ActionCommand(SaveTemplateCommand_Executed, SaveTemplateCommand_CanExecute);

	private ICommand openBinaryFileCommand;

	public ICommand OpenBinaryFileCommand =>
		openBinaryFileCommand ??= new ActionCommand(OpenBinaryFileCommand_Executed, OpenBinaryFileCommand_CanExecute);

	private ICommand closeTabCommand;

	public ICommand CloseTabCommand => closeTabCommand ??= new ActionCommand(CloseTabCommand_Executed, CloseTabCommand_CanExecute);

	private ICommand closeCommand;

	public ICommand CloseCommand => closeCommand ??= new ActionCommand(CloseCommand_Executed, CloseCommand_CanExecute);

	#endregion commands

	#region command handlers

	private bool NewTemplateCommand_CanExecute(object parameter) => true;

	private void NewTemplateCommand_Executed(object parameter)
	{
		BinaryTemplateTabViewModel newTab = new("new template");
		AddNewTab(newTab);
	}

	private bool LoadTemplateCommand_CanExecute(object parameter) => true;

	private void LoadTemplateCommand_Executed(object parameter)
	{
		FileDialogEventArgs fileDialogEventArgs = new()
		{
			Title = TranslationManager.GetResourceText("SelectTemplate"),
			Filter = TranslationManager.GetResourceText("FileDialogFilter_Templates") + " (*.xml)|*.xml"
		};

		FileDialogRequested?.Invoke(this, fileDialogEventArgs);

		string templateFile = fileDialogEventArgs.File;
		if (!string.IsNullOrWhiteSpace(templateFile))
		{
			BinaryTemplateTabViewModel loadedTemplate = new(templateFile);

			if (loadedTemplate.LoadTemplateFromFile(templateFile))
			{
				AddNewTab(loadedTemplate);
			}
			// TODO!
		}
	}

	private bool SaveTemplateCommand_CanExecute(object parameter)
	{
		if (SelectedTabIndex > -1 && SelectedTabIndex < TabVMs.Count)
		{
			return TabVMs[SelectedTabIndex] is BinaryTemplateTabViewModel;
		}

		return false;
	}

	private void SaveTemplateCommand_Executed(object parameter)
	{
		if (TabVMs.Count > SelectedTabIndex && TabVMs[SelectedTabIndex] is BinaryTemplateTabViewModel templateTabViewModel)
		{
			FileDialogEventArgs fileDialogEventArgs = new()
			{
				Title = TranslationManager.GetResourceText("SaveTemplate"),
				Filter = TranslationManager.GetResourceText("FileDialogFilter_Templates") + " (*.xml)|*.xml"
			};

			FileDialogRequested?.Invoke(this, fileDialogEventArgs);

			string templateFile = fileDialogEventArgs.File;
			if (!string.IsNullOrWhiteSpace(templateFile))
			{
				if (templateTabViewModel.SaveTemplateToFile(templateFile))
				{
					// TODO
				}
				// TODO
			}
		}
	}

	private bool OpenBinaryFileCommand_CanExecute(object parameter) => true;

	private void OpenBinaryFileCommand_Executed(object parameter)
	{
		// check for list of files in command parameter:
		string binaryFile = null;
		if (parameter != null)
		{
			binaryFile = parameter as string;
		}

		// if no file in parameters -> select binary file via dialog:
		if (string.IsNullOrWhiteSpace(binaryFile))
		{
			FileDialogEventArgs fileDialogBinaryEventArgs = new()
			{
				Title = TranslationManager.GetResourceText("SelectBinaryFile"),
				Filter = TranslationManager.GetResourceText("FileDialogFilter_Binary") + " (*.*)|*.*"
			};

			FileDialogRequested?.Invoke(this, fileDialogBinaryEventArgs);

			binaryFile = fileDialogBinaryEventArgs.File;
		}

		if (!string.IsNullOrWhiteSpace(binaryFile))
		{
			// first check if there is any matching template in default template directory:
			BinaryDataTemplate template = templateManager.GetMatchingTemplate(binaryFile);

			if (template == null)
			{
				// otherwise -> select template file via dialog:
				FileDialogEventArgs fileDialogTemplateEventArgs = new()
				{
					Title = TranslationManager.GetResourceText("SelectTemplate"),
					Filter = TranslationManager.GetResourceText("FileDialogFilter_Templates") + " (*.xml)|*.xml"
				};

				FileDialogRequested?.Invoke(this, fileDialogTemplateEventArgs);

				string templateFile = fileDialogTemplateEventArgs.File;
				if (!string.IsNullOrWhiteSpace(templateFile))
				{
					template = templateManager.GetTemplate(templateFile);
				}
			}

			if (template != null)
			{
				string tabHeader = "";
				try
				{
					tabHeader = Path.GetFileName(binaryFile);
				}
				catch
				{
					tabHeader = binaryFile;
				}

				BinaryDataTabViewModel newTab = new(tabHeader, binaryFile, template);
				AddNewTab(newTab);
			}
		}

		// TODO: stattdessen eigener Dialog zur Auswahl von Binärdatei und Template!

		// OpenBinaryFileWindowEventArgs eventArgs = new OpenBinaryFileWindowEventArgs()
		// OpenBinaryFileWindowRequested?.Invoke(this, eventArgs);

		// if (eventArgs.DialogResult)
		// {
		//     string binaryFile = eventArgs.BinaryFilePath;
		// 	string templateFile = eventArgs.TemplatePath;
		//     var newTab = new BinaryDataTabViewModel("binary file", binaryFile, templateFile);
		//     AddNewTab(newTab);
		// }
	}

	private bool CloseTabCommand_CanExecute(object parameter) => true;

	private void CloseTabCommand_Executed(object parameter)
	{
		CloseTab(parameter as TabViewModelBase);
	}

	private bool CloseCommand_CanExecute(object parameter) => true;

	private void CloseCommand_Executed(object parameter)
	{
		CloseRequested?.Invoke(this, null);
	}

	#endregion command handlers

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

	#endregion private methods
}