using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using BinaryDataReaderApp.Events;
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
			// ViewModel.OpenBinaryFileWindowRequested += OnOpenBinaryFileWindowRequested;
		}

		private void OnFileDialogRequested(object sender, FileDialogEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog()
			{
				Title = e.Title,
				Filter = e.Filter
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

		private void OnOpenBinaryFileWindowRequested(object sender, OpenBinaryFileWindowEventArgs e)
		{
			OpenBinaryFileWindowView openBinaryFileWindow = new OpenBinaryFileWindowView();

			openBinaryFileWindow.ShowDialog();
			bool dialogResult = openBinaryFileWindow.ViewModel.DialogResult;

			if (dialogResult)
			{
				e.BinaryFilePath = "";
				e.TemplatePath = "";
				e.DialogResult = true;
			}
			else
			{
				e.BinaryFilePath = "";
				e.TemplatePath = "";
				e.DialogResult = false;
			}
		}

		private void Window_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effects = DragDropEffects.Copy;
			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

				if ((files != null) && files.Any())
				{
					if (ViewModel.OpenBinaryFileCommand.CanExecute(files[0]))
					{
						ViewModel.OpenBinaryFileCommand.Execute(files[0]);
					}
				}
			}
		}
	}
}
