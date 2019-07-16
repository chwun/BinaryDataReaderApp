using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Models;
using BinaryDataReaderApp.Localization;

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
        public delegate void OpenBinaryFileWindowRequestedEventHandler(object sender, OpenBinaryFileWindowEventArgs e);

        public event FileDialogRequestedEventHandler FileDialogRequested;
        public event CloseRequestedEventHandler CloseRequested;
        public event OpenBinaryFileWindowRequestedEventHandler OpenBinaryFileWindowRequested;

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

        private ICommand openBinaryFileCommand;
        public ICommand OpenBinaryFileCommand
        {
            get
            {
                if (openBinaryFileCommand == null)
                {
                    openBinaryFileCommand = new ActionCommand(OpenBinaryFileCommand_Executed, OpenBinaryFileCommand_CanExecute);
                }

                return openBinaryFileCommand;
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
            FileDialogEventArgs fileDialogEventArgs = new FileDialogEventArgs()
            {
                Title = TranslationManager.Instance.GetResourceText("SelectTemplate"),
                Filter = TranslationManager.Instance.GetResourceText("FileDialogFilter_Templates") + " (*.xml)|*.xml"
            };

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

        private bool OpenBinaryFileCommand_CanExecute(object parameter)
        {
            return true;
        }

        private void OpenBinaryFileCommand_Executed(object parameter)
        {
            // select binary file:
            FileDialogEventArgs fileDialogBinaryEventArgs = new FileDialogEventArgs()
            {
                Title = TranslationManager.Instance.GetResourceText("SelectBinaryFile"),
                Filter = TranslationManager.Instance.GetResourceText("FileDialogFilter_Binary") + " (*.*)|*.*"
            };

            FileDialogRequested?.Invoke(this, fileDialogBinaryEventArgs);

            string binaryFile = fileDialogBinaryEventArgs.File;
            if (!string.IsNullOrWhiteSpace(binaryFile))
            {
                // select template:
                FileDialogEventArgs fileDialogTemplateEventArgs = new FileDialogEventArgs()
                {
                    Title = TranslationManager.Instance.GetResourceText("SelectTemplate"),
                    Filter = TranslationManager.Instance.GetResourceText("FileDialogFilter_Templates") + " (*.xml)|*.xml"
                };

                FileDialogRequested?.Invoke(this, fileDialogTemplateEventArgs);

                string templateFile = fileDialogTemplateEventArgs.File;
                if (!string.IsNullOrWhiteSpace(templateFile))
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

                    var newTab = new BinaryDataTabViewModel(tabHeader, binaryFile, templateFile);
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
            CloseRequested?.Invoke(this, null);
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