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
	/// Interaction logic for MainWindowView.xaml
	/// </summary>
	public partial class MainWindowView : Window
	{
		protected MainViewModel ViewModel;

		public MainWindowView()
		{
			InitializeComponent();

			TranslationManager.Instance.SetLanguage(CultureInfo.GetCultureInfo("en-US")); // TODO - Workaround für Bug TabControl!!!

			ViewModel = new MainViewModel();
			DataContext = ViewModel;

			ViewModel.FileDialogRequested += OnFileDialogRequested;
			ViewModel.CloseRequested += OnCloseRequested;
		}

		private void OnFileDialogRequested(object sender, FileDialogEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog()
			{
				Filter = "Template files (*.xml)|*.xml"
			};

			if (dlg.ShowDialog() == true)
			{
				e.File = dlg.FileName;
			}
		}

		private void OnCloseRequested(object sender, EventArgs e)
		{
			Close();
		}
	}
}
