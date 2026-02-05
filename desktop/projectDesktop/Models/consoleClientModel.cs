using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using projectDesktop.DTOs;

namespace projectDesktop.Models;

public class consoleClientModel
{
    private HttpClient _httpClient;

    public consoleClientModel(string port)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(port)
        };
    }
    
    public async Task<List<SearchUserDTO>> SearchUsers(string query)
    {
        return await _httpClient.GetFromJsonAsync<List<SearchUserDTO>>($"api/news/search_user?name={query}");
    }

    public async Task<List<PostDTO>> SearchPosts(string query)
    {
        return await _httpClient.GetFromJsonAsync<List<PostDTO>>($"api/news/search_post?title={query}");
    }

    public async Task DeleteUser(int id)
    {
        var resp= await _httpClient.DeleteAsync($"api/news/delete_users/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task ModifyUser(ModifyUserDTO modifyUser)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/modify_users", modifyUser);
        resp.EnsureSuccessStatusCode();
    }

    public async Task CreatePost(PostDTO post)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/create_posts", post);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeletePost(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_posts/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteOwnPost(DeleteOwnPostDTO deleteOwnPost)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_own_posts/{deleteOwnPost.id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task FavouritePosts(FavouritePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/favourite_posts", dto);
        resp.EnsureSuccessStatusCode();
    }

    public async Task UnFavouritePosts(FavouritePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/unfavourite_posts", dto);
        resp.EnsureSuccessStatusCode();
    }

    public async Task UpVotePost(UpVotePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/upvote/", dto);
        resp.EnsureSuccessStatusCode();
    }
    
    public async Task DownVotePost(DownVotePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/downvote/", dto);
        resp.EnsureSuccessStatusCode();
    }

    public async Task Comment(CommentDTO comment)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/comment", comment);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteComment(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_comment/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task CreateCategory(CategoryDTO category)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/create_category", category);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategory(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_category/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task CreateReport(ReportDTO report)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/create_report", report);
        resp.EnsureSuccessStatusCode();
    }
}
