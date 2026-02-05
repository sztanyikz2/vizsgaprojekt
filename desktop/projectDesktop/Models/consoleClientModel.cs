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
}
