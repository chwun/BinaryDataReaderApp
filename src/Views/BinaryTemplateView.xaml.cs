using System;
using System.Windows;
using System.Windows.Controls;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.ViewModels;

namespace BinaryDataReaderApp.Views
{
	/// <summary>
	/// Interaction logic for BinaryTemplateView.xaml
	/// </summary>
	public partial class BinaryTemplateView : UserControl
	{
		protected BinaryTemplateTabViewModel ViewModel
		{
			get
			{
				return DataContext as BinaryTemplateTabViewModel;
			}
		}

		public BinaryTemplateView()
		{
			InitializeComponent();
		}

		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.TemplatePartDetailsWindowRequested += OnTemplatePartDetailsWindowRequested;
			}
		}

		private void OnTemplatePartDetailsWindowRequested(object sender, TemplatePartDetailsWindowEventArgs e)
		{
			TemplatePartDetailsWindowView templatePartDetailsWindow = new TemplatePartDetailsWindowView(e);
			templatePartDetailsWindow.Owner = MainWindowView.MainWindowInstance;

			templatePartDetailsWindow.ShowDialog();
		}
	}
}