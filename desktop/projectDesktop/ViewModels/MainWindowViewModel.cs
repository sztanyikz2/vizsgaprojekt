namespace projectDesktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string TopPostLinks { get; } = "toppostlinks placeholder";
    public string CategoryLinks  { get; } = "categorylinks placeholder";
    public string FeedItems { get; }  = "feeditems placeholder";
    public string SearchText { get; } = "searchtext placeholder";
    public double ThemeStrengthSliderValue { get; set; }
}