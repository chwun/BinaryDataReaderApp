using System.Windows.Input;

namespace BinaryDataReaderApp.Common;

public class ActionCommand : ICommand
{
	private readonly Func<object, bool> canExecuteHandler;
	private readonly Action<object> executeHandler;

	public ActionCommand(Action<object> executeHandler)
	{
		this.executeHandler = executeHandler;
	}

	public ActionCommand(Action<object> execute, Func<object, bool> canExecute)
		: this(execute)
	{
		canExecuteHandler = canExecute;
	}

	public event EventHandler CanExecuteChanged
	{
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	public void Execute(object parameter)
	{
		executeHandler(parameter);
	}

	public bool CanExecute(object parameter) => canExecuteHandler?.Invoke(parameter) ?? true;
}