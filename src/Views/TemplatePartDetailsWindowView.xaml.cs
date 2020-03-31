using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Localization;
using BinaryDataReaderApp.ViewModels;
using Microsoft.Win32;

namespace BinaryDataReaderApp.Views
{
	/// <summary>
	/// Interaction logic for OpenBinaryFileWindowView.xaml
	/// </summary>
	public partial class TemplatePartDetailsWindowView : Window
	{
		public TemplatePartDetailsWindowViewModel ViewModel
		{
			get;
			protected set;
		}

		public TemplatePartDetailsWindowView(TemplatePartDetailsWindowEventArgs e)
		{
			InitializeComponent();

			ViewModel = new TemplatePartDetailsWindowViewModel(e);
			DataContext = ViewModel;

			ViewModel.CloseRequested += OnCloseRequested;

			Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}

		private void OnCloseRequested(object sender, EventArgs e)
		{
			Close();
		}

		private void TextBoxPartName_SourceUpdated(object sender, DataTransferEventArgs e)
		{
			var textBox = sender as TextBox;

			if (textBox != null)
			{
				textBox.CaretIndex = textBox.Text.Length;
			}
		}
	}
}
