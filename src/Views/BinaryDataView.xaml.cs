using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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
	}
}