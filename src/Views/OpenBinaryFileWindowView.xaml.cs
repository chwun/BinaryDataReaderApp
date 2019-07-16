using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using BinaryDataReaderApp.Localization;
using BinaryDataReaderApp.ViewModels;
using Microsoft.Win32;

namespace BinaryDataReaderApp.Views
{
    /// <summary>
    /// Interaction logic for OpenBinaryFileWindowView.xaml
    /// </summary>
    public partial class OpenBinaryFileWindowView : Window
    {
        public OpenBinaryFileWindowViewModel ViewModel
        {
            get;
            protected set;
        }

        public OpenBinaryFileWindowView()
        {
            InitializeComponent();

            ViewModel = new OpenBinaryFileWindowViewModel();
            DataContext = ViewModel;

            ViewModel.CloseRequested += OnCloseRequested;
        }

        private void OnCloseRequested(object sender, EventArgs e)
        {
            Close();
        }
    }
}
