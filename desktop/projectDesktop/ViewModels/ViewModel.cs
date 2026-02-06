using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace projectDesktop.ViewModels;

public partial class ViewModel : ObservableObject
{
    [ObservableProperty] private double _sliderValue;
}