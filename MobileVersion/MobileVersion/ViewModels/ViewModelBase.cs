using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileVersion.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    protected ViewModelBase()
    {
    }
    public event PropertyChangingEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged98([CallerMemberName] String? propertyName = null)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangingEventArgs(propertyName));
        }

    }
}
