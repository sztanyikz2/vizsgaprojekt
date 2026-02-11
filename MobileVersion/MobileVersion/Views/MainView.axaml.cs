using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MobileVersion.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    private void OpenMenu(object? sender, RoutedEventArgs e)
    {
        SideMenu.IsVisible = true;
    }

    private void CloseMenu(object? sender, RoutedEventArgs e)
    {
        SideMenu.IsVisible = false;
    }
}