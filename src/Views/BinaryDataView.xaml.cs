using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.ViewModels;

namespace BinaryDataReaderApp.Views
{
    /// <summary>
    /// Interaction logic for BinaryDataView.xaml
    /// </summary>
    public partial class BinaryDataView : UserControl
    {
        protected BinaryDataTabViewModel ViewModel
        {
            get
            {
                return DataContext as BinaryDataTabViewModel;
            }
        }

        public BinaryDataView()
        {
            InitializeComponent();
        }

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

            if (cellInfo != null)
            {
                object selectedRowItem = cellInfo.Item;
                int selectedColumnIndex = cellInfo.Column?.DisplayIndex ?? -1;
                ViewModel?.SetSelectionInTree(selectedRowItem, selectedColumnIndex);
            }
        }

        private void OnHexDumpSelectionChanged(object sender, HexDumpSelectionChangedEventArgs e)
        {
            dataGridHexDump.ScrollIntoView(e.SelectedHexDumpLine);
        }
    }
}