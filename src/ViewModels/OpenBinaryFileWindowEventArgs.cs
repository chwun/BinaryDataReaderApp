using System;

namespace BinaryDataReaderApp.ViewModels
{
    public class OpenBinaryFileWindowEventArgs : EventArgs
    {
        public bool DialogResult { get; set; }

        public string TemplatePath { get; set; }

        public string BinaryFilePath { get; set; }
    }
}