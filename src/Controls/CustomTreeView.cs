using BinaryDataReaderApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace BinaryDataReaderApp.Controls;

public class CustomTreeView : TreeView
{
	public static readonly DependencyProperty SelectedTreeItemProperty = DependencyProperty.Register("SelectedTreeItem", typeof(BinaryPart),
		typeof(CustomTreeView),
		new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public CustomTreeView()
	{
		SelectedItemChanged += OnSelectedItemChanged;
	}

	public BinaryPart SelectedTreeItem
	{
		get => GetValue(SelectedTreeItemProperty) as BinaryPart;
		set => SetValue(SelectedTreeItemProperty, value);
	}

	private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		SelectedTreeItem = e.NewValue as BinaryPart;
	}
}