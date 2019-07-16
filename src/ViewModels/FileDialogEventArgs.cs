using System;

namespace BinaryDataReaderApp.ViewModels
{
    public class FileDialogEventArgs : EventArgs
    {
        public string Title { get; set; }

        public string Filter { get; set; }

        public string File { get; set; }
    }
}