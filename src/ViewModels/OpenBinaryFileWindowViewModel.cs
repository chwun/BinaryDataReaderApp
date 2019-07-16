using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BinaryDataReaderApp.Common;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.ViewModels
{
    public class OpenBinaryFileWindowViewModel : ViewModelBase
    {
        private string templatePath;
        private string binaryFilePath;

        public string TemplatePath
        {
            get
            {
                return templatePath;
            }
            set
            {
                templatePath = value;
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

        public bool DialogResult { get; private set; }

        #region events

        public delegate void CloseRequestedEventHandler(object sender, EventArgs e);
        public event CloseRequestedEventHandler CloseRequested;

        #endregion

        public OpenBinaryFileWindowViewModel()
        {

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
            DialogResult = true;
            CloseRequested?.Invoke(this, null);
        }

        #endregion

        #region private methods
        
        #endregion
    }
}