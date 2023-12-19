using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BinaryDataReaderApp.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new(propertyName));
	}
}