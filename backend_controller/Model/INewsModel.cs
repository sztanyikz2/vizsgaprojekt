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
        public void FavouritePost(FavouritePostDTO source);
        public void UnfavouritePost(UnfavouritePostDTO source);
        public void voteOnPost(VoteDTO source);
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
