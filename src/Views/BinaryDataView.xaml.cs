using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BinaryDataReaderApp.Views;

/// <summary>
/// Interaction logic for BinaryDataView.xaml
/// </summary>
public partial class BinaryDataView : UserControl
{
	public BinaryDataView()
	{
		InitializeComponent();
	}

	protected BinaryDataTabViewModel ViewModel => DataContext as BinaryDataTabViewModel;

	private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (ViewModel?.BinaryFile != null)
		{
			ViewModel.BinaryFile.HexDumpSelectionChanged += OnHexDumpSelectionChanged;
		}
	}

	private void DataGridHexDump_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
	{
		DataGridCellInfo cellInfo = e.AddedCells.FirstOrDefault();

		object selectedRowItem = cellInfo.Item;
		int selectedColumnIndex = cellInfo.Column?.DisplayIndex ?? -1;
		ViewModel?.SetSelectionInTree(selectedRowItem, selectedColumnIndex);
	}

	private void OnHexDumpSelectionChanged(object sender, HexDumpSelectionChangedEventArgs e)
	{
		dataGridHexDump.ScrollIntoView(e.SelectedHexDumpLine);
	}
}