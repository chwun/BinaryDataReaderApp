using System.Windows.Input;

namespace BinaryDataReaderApp
{
	public static class CustomCommands
	{
		public static readonly RoutedUICommand NewTemplateCommand = new RoutedUICommand
			(
				"NewTemplate",
				"NewTemplate",
				typeof(CustomCommands),
				new InputGestureCollection()
				{

				}
			);
	}
}