using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileVersion.DTOs;
using MobileVersion.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MobileVersion.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly consoleClientModel _model;
    public ObservableCollection<string> Categories { get; set; }
    public void LoadCategories()
    {
        Categories.Clear();
        List<string> categories = new List<string>
        {
            "Gaming",
            "Shows/Movies",
            "Politics",
            "Animals",
            "Books",
            "Health",
            "Tech/Gadgets",
            "Food/Cooking"
        };
        categories.ForEach(c => Categories.Add(c));
    }
    public RelayCommand DisplayCategory { get; set; }
    public MainViewModel(consoleClientModel model)
    {
        _model = model;
        Categories = new ObservableCollection<string>();
        LoadCategories();
    }
}
