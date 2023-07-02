using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CMM.Library.Base;

public class PropertyBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    virtual internal protected void OnPropertyChanged(string propertyName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (object.Equals(storage, value)) return;

        storage = value;
        this.OnPropertyChanged(propertyName);
    }
}
