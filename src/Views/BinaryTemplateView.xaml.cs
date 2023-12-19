using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BinaryDataReaderApp.Views;

/// <summary>
/// Interaction logic for BinaryTemplateView.xaml
/// </summary>
public partial class BinaryTemplateView : UserControl
{
	public BinaryTemplateView()
	{
		InitializeComponent();
	}

	protected BinaryTemplateTabViewModel ViewModel => DataContext as BinaryTemplateTabViewModel;

	private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (ViewModel != null)
		{
			ViewModel.TemplatePartDetailsWindowRequested += OnTemplatePartDetailsWindowRequested;
		}
	}

	private void OnTemplatePartDetailsWindowRequested(object sender, TemplatePartDetailsWindowEventArgs e)
	{
		TemplatePartDetailsWindowView templatePartDetailsWindow = new(e);
		templatePartDetailsWindow.Owner = MainWindowView.MainWindowInstance;

		templatePartDetailsWindow.ShowDialog();
	}
}