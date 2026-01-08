using Avalonia.Controls;
using Avalonia.Interactivity;

namespace project_mobil.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OpenPane_Click(object? sender, RoutedEventArgs e)
    {
        MainSplitView.IsPaneOpen = true;
    }

    private void ClosePane_Click(object? sender, RoutedEventArgs e)
    {
        MainSplitView.IsPaneOpen = false;
    }
}
