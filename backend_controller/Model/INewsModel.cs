using vizsgaController.Dtos;

namespace vizsgaController.Model
{
    public interface INewsModel
    {
        //search
        public IEnumerable<UserDTO> GetUserNamesBySearch(string name);
        public IEnumerable<PostDTO> GetPostsBySearch(string title);
        //users
        public void DeleteUsers(int id);
        public void ModifyUsers(ModifyUserDTO  source);
        //posts
        public void CreatePost(PostDTO source);
        public void DeletePost(int id);
        public void DeleteOwnPost(DeleteOwnPostDTO source);
        public void FavouritePost(int postId, int userId);
        public void UnfavouritePost(int postId, int userId);
        public void UpVoteOnPost(int postId, int userid);
        public void DownVoteOnPost(int postId, int userid);
        //coment
        public void CommentOnPost(CommentDTO source);
        public void DeleteComments(int id);
        //categories
        public void CreateCategory(CategoryDTO source);
        public void DeleteCategory(int id);
        //reports
        public void CreateReport(ReportDTO source);
        //smth
        public void ManageSiteSettings();
        

    }
}
