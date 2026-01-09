using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection
{
    public class model
    {
        private readonly HttpClient client;
        public model(string port)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(port)
            };
        }

        public Task<List<UserDTO>> GetUsernamBySearch(string search)
        {
            return client.GetFromJsonAsync<List<UserDTO>>($"/api/vizsgacontroller/searchuser/{search}");
        }
        public Task<List<PostDTO>> GetPostsBySearch(string search)
        {
            return client.GetFromJsonAsync<List<POstDTO>>($"/api/vizsgacontroller/searchpost/{search}");
        }
        public async Task DeletePost(int id)
        {
            var response = await client.DeleteAsync($"/api/vizsgacontroller/deletepost/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task ModerateComments(int id)
        {
            var response = await client.DeleteAsync($"/api/vizsgacontroller/moderatecomments/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteOwnPost(int id)
        {
            var response = await client.DeleteAsync($"/api/vizsgacontroller/deleteownpost/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task FavouritePost(int id)
        {
            var response = await client.PostAsync($"/api/vizsgacontroller/favouriteposts/{id}", null);
            response.EnsureSuccessStatusCode();
        }
        public async Task ManageUsers(int id)
        {
            var response = await client.DeleteAsync($"/api/vizsgacontroller/manageusers/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task ModifyUsers(int id)
        {
            var response = await client.PutAsync($"/api/vizsgacontroller/modifyusers/{id}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
