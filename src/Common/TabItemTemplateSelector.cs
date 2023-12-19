using BinaryDataReaderApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BinaryDataReaderApp.Common;

public class TabItemTemplateSelector : DataTemplateSelector
{
	public DataTemplate TabTemplate_BinaryTemplate { get; set; }
	public DataTemplate TabTemplate_BinaryData { get; set; }

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is BinaryTemplateTabViewModel)
		{
			return TabTemplate_BinaryTemplate;
		}

		if (item is BinaryDataTabViewModel)
		{
			return TabTemplate_BinaryData;
		}

		return base.SelectTemplate(item, container);
	}
}