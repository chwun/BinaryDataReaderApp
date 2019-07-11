using System.Windows;
using System.Windows.Controls;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.Controls
{
    public class CustomTreeView : TreeView
    {
        public static readonly DependencyProperty SelectedTreeItemProperty = DependencyProperty.Register("SelectedTreeItem", typeof(BinaryPart), typeof(CustomTreeView),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		
		public BinaryPart SelectedTreeItem
		{
			get
			{
				return this.GetValue(SelectedTreeItemProperty) as BinaryPart;
			}
			set
			{
				this.SetValue(SelectedTreeItemProperty, value);
			}
		}

		public CustomTreeView()
		{
			SelectedItemChanged += OnSelectedItemChanged;
		}

		private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			SelectedTreeItem = e.NewValue as BinaryPart;
		}
    }
}