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
	}
}