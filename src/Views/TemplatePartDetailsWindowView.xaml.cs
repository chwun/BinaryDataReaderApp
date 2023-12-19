using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BinaryDataReaderApp.Views;

/// <summary>
/// Interaction logic for OpenBinaryFileWindowView.xaml
/// </summary>
public partial class TemplatePartDetailsWindowView : Window
{
	public TemplatePartDetailsWindowView(TemplatePartDetailsWindowEventArgs e)
	{
		InitializeComponent();

		ViewModel = new(e);
		DataContext = ViewModel;

		ViewModel.CloseRequested += OnCloseRequested;

		Loaded += (sender, e) => MoveFocus(new(FocusNavigationDirection.First));
	}

	public TemplatePartDetailsWindowViewModel ViewModel { get; protected set; }

	private void OnCloseRequested(object sender, EventArgs e)
	{
		Close();
	}

	private void TextBoxPartName_SourceUpdated(object sender, DataTransferEventArgs e)
	{
		TextBox textBox = sender as TextBox;

		if (textBox != null)
		{
			textBox.CaretIndex = textBox.Text.Length;
		}
	}
}