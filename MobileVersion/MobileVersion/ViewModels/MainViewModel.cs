using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileVersion.DTOs;
using MobileVersion.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MobileVersion.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly consoleClientModel _model;
    public ObservableCollection<string> Categories { get; set; }
    public ObservableCollection<string> SortBy { get; set; }
    public void LoadDefault()
    {
        Categories.Clear();
        SortBy.Clear();
        List<string> categories = new List<string>
        {
            "All",
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
        List<string> sorting = new List<string>
        {
            "All",
            "Newest",
            "Oldest",
            "Most Liked",
            "Most Disliked"

        };
        sorting.ForEach(c => SortBy.Add(c));
    }
    public ObservableCollection<Post> AllPosts { get; } =
        new ObservableCollection<Post>
        {
            new Post("AI Breakthrough", "Books"),
            new Post("Football Finals", "Gaming"),
            new Post("Elections Update", "Health"),
            new Post("New Smartphone", "Politics")
        };
    private ObservableCollection<Post> _filteredposts;
    public ObservableCollection<Post> FilteredPosts
    {
        get => _filteredposts;
        set
        {
            if (_filteredposts != value)
            {
                _filteredposts = value;
                OnPropertyChanged(nameof(FilteredPosts));
            }
        }
    }


    private string _selectedCategory;
    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterPosts();
            }
        }
    }

    private void FilterPosts()
    {
        if (SelectedCategory == "All")
        {
            FilteredPosts = new ObservableCollection<Post>(AllPosts);
        }
        else
        {
            FilteredPosts = new ObservableCollection<Post>(AllPosts.Where(p => p.Category == SelectedCategory));
        }
    }



    public RelayCommand DisplayCategory { get; set; }
    public MainViewModel(consoleClientModel model)
    {
        _model = model;
        Categories = new ObservableCollection<string>();
        SortBy = new ObservableCollection<string>();
        FilteredPosts = new ObservableCollection<Post>();
        SelectedCategory = "All";
        LoadDefault();
    }
}
public class Post
{
    public string Title { get; }
    public string Category { get; }

    public Post(string title, string category)
    {
        Title = title;
        Category = category;
    }
}
