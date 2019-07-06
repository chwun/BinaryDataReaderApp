using System.Windows;
using System.Windows.Controls;
using BinaryDataReaderApp.ViewModels;

namespace BinaryDataReaderApp.Common
{
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
			else if (item is BinaryDataTabViewModel)
			{
				return TabTemplate_BinaryData;
			}
			else
			{
				return base.SelectTemplate(item, container);
			}
		}
	}
}