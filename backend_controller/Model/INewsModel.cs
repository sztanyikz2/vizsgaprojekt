using vizsgaController.Dtos;

namespace vizsgaController.Model
{
    public interface INewsModel
    {
        //search
        public IEnumerable<UserDTO> GetUserNamesBySearch(string name);
        public IEnumerable<PostDTO> GetPostsBySearch(string title);
        //users
        public Task DeleteUsers(int id);
        public Task ModifyUsers(ModifyUserDTO  source);
        //posts
        public Task CreatePost(PostDTO source);
        public Task DeletePost(int id);
        public Task DeleteOwnPost(DeleteOwnPostDTO source);
        public Task FavouritePost(FavouritePostDTO source);
        public Task voteOnPost(VoteDTO source);
        //coment
        public Task CommentOnPost(CommentDTO source);
        public Task DeleteComments(int id);
        //categories
        public Task CreateCategory(CategoryDTO source);
        public Task DeleteCategory(int id);
        //reports
        public Task CreateReport(ReportDTO source);
        //smth
        public void ManageSiteSettings();
        

    }
}
