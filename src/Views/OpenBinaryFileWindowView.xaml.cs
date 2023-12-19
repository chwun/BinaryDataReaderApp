using BinaryDataReaderApp.ViewModels;
using System.Windows;

namespace BinaryDataReaderApp.Views;

/// <summary>
/// Interaction logic for OpenBinaryFileWindowView.xaml
/// </summary>
public partial class OpenBinaryFileWindowView : Window
{
	public OpenBinaryFileWindowView()
	{
		InitializeComponent();

		ViewModel = new();
		DataContext = ViewModel;

		ViewModel.CloseRequested += OnCloseRequested;
	}

	public OpenBinaryFileWindowViewModel ViewModel { get; protected set; }

	private void OnCloseRequested(object sender, EventArgs e)
	{
		Close();
	}
}