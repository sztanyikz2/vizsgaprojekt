using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MobileVersion.DTOs;

namespace MobileVersion.Models;

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
    
    public async Task<List<SearchUserDTO>> searchUsers(string query)
    {
        return await _httpClient.GetFromJsonAsync<List<SearchUserDTO>>($"api/news/search_user?name={query}");
    }

    public async Task<List<PostDTO>> searchPosts(string query)
    {
        return await _httpClient.GetFromJsonAsync<List<PostDTO>>($"api/news/search_post?title={query}");
    }

    public async Task deleteUser(int id)
    {
        var resp= await _httpClient.DeleteAsync($"api/news/delete_users/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task modifyUser(ModifyUserDTO modifyUser)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/modify_users", modifyUser);
        resp.EnsureSuccessStatusCode();
    }

    public async Task createPost(PostDTO post)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/create_posts", post);
        resp.EnsureSuccessStatusCode();
    }

    public async Task deletePost(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_posts/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task deleteOwnPost(DeleteOwnPostDTO deleteOwnPost)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_own_posts/{deleteOwnPost.id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task favouritePosts(FavouritePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/favourite_posts", dto);
        resp.EnsureSuccessStatusCode();
    }

    public async Task unFavouritePosts(FavouritePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/unfavourite_posts", dto);
        resp.EnsureSuccessStatusCode();
    }

    public async Task votePost(votePostDTO dto)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/news/vote/", dto);
        resp.EnsureSuccessStatusCode();
    }

    public async Task comment(CommentDTO comment)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/comment", comment);
        resp.EnsureSuccessStatusCode();
    }

    public async Task deleteComment(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_comment/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task createCategory(CategoryDTO category)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/create_category", category);
        resp.EnsureSuccessStatusCode();
    }

    public async Task deleteCategory(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/news/delete_category/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task createReport(ReportDTO report)
    {
        var resp = await _httpClient.PostAsJsonAsync($"api/news/create_report", report);
        resp.EnsureSuccessStatusCode();
    }
}
