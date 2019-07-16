using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BinaryDataReaderApp.ViewModels;

namespace BinaryDataReaderApp.Views
{
	/// <summary>
	/// Interaction logic for BinaryDataView.xaml
	/// </summary>
	public partial class BinaryDataView : UserControl
	{
		protected BinaryDataTabViewModel ViewModel;

		public BinaryDataView()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty TabDataProperty = DependencyProperty.Register("TabData", typeof(BinaryDataTabViewModel), typeof(BinaryDataView));
		public BinaryDataTabViewModel TabData
		{
			get
			{
				return this.GetValue(TabDataProperty) as BinaryDataTabViewModel;
			}
			set
			{
				this.SetValue(TabDataProperty, value);
				ViewModel = value;
				DataContext = ViewModel;
			}
		}

		private void treeView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (sender is TreeView && !e.Handled)
			{
				e.Handled = true;
				var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
				eventArg.RoutedEvent = UIElement.MouseWheelEvent;
				eventArg.Source = sender;
				var parent = ((Control)sender).Parent as UIElement;
				parent.RaiseEvent(eventArg);
			}
		}
	}
}