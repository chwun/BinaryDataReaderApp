using System.Windows;
using System.Windows.Controls;
using BinaryDataReaderApp.ViewModels;

namespace BinaryDataReaderApp.Views
{
	/// <summary>
	/// Interaction logic for BinaryTemplateView.xaml
	/// </summary>
	public partial class BinaryTemplateView : UserControl
	{
		protected BinaryTemplateTabViewModel ViewModel;

		public BinaryTemplateView()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty TabDataProperty = DependencyProperty.Register("TabData", typeof(BinaryTemplateTabViewModel), typeof(BinaryTemplateView));
		public BinaryTemplateTabViewModel TabData
		{
			get
			{
				return this.GetValue(TabDataProperty) as BinaryTemplateTabViewModel;
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