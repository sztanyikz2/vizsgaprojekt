using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileVersion.DTOs;
using MobileVersion.Models;
using System;
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
    public ObservableCollection<Post> AllPosts { get; set;}
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
    private string _sort;
    public string Sort
    {
        get => _sort;
        set
        {
            if (_sort != value)
            {
                _sort = value;
                OnPropertyChanged(nameof(Sort));
                FilterPosts();
            }
        }
    }

    private void FilterPosts()
    {
        IEnumerable<Post> posts = AllPosts;

        // Filter by category (error handling too)
        if (!string.IsNullOrEmpty(SelectedCategory) && !SelectedCategory.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            posts = posts.Where(p => p.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase));
        }

        // Sort
        if (!string.IsNullOrEmpty(Sort) && !Sort.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            posts = Sort switch
            {
                "Newest" => posts.OrderByDescending(p => p.DateCreated),
                "Oldest" => posts.OrderBy(p => p.DateCreated),
                "Most Liked" => posts.OrderByDescending(p => p.Likes),
                "Most Disliked" => posts.OrderByDescending(p => p.Dislikes),
                _ => posts
            };
        }
        FilteredPosts = new ObservableCollection<Post>(posts);
    }




    public RelayCommand DisplayCategory { get; set; }
    public MainViewModel(consoleClientModel model)
    {
        _model = model;
        Categories = new ObservableCollection<string>();
        SortBy = new ObservableCollection<string>();
        FilteredPosts = new ObservableCollection<Post>();
        AllPosts = new ObservableCollection<Post>
        {
            new Post("AI Breakthrough", "Books", DateTime.Now.AddDays(-1), 12, 2),
            new Post("Football Finals", "Gaming", DateTime.Now.AddDays(-3), 30, 5),
            new Post("Elections Update", "Health", DateTime.Now.AddDays(-5), 20, 10),
            new Post("New Smartphone", "Politics", DateTime.Now.AddDays(-2), 15, 1),
            new Post("AI lamoi", "Books", DateTime.Now.AddDays(-6), 12, 2),
            new Post("Football Finvfafvals", "Gaming", DateTime.Now.AddDays(-8), 30, 5),
            new Post("Elections vevrfeefv", "Health", DateTime.Now.AddDays(-9), 20, 10),
            new Post("New verfvverf", "Politics", DateTime.Now.AddDays(-11), 15, 1)
        };
        LoadDefault();
        FilterPosts();
        SelectedCategory = "All";
        Sort = "All";
    }
}
public class Post
{
    public string Title { get; }
    public string Category { get; }
    public DateTime DateCreated { get; }  // for Newest/Oldest
    public int Likes { get; }             // for Most Liked
    public int Dislikes { get; }          // for Most Disliked

    public Post(string title, string category, DateTime dateCreated, int likes, int dislikes)
    {
        Title = title;
        Category = category;
        DateCreated = dateCreated;
        Likes = likes;
        Dislikes = dislikes;
    }
}

