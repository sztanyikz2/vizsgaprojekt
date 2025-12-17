using vizsgaController.Dtos;

namespace vizsgaController.Model
{
    public interface INewsModel
    {
        public IEnumerable<UserDTO> GetUserNamesBySearch(string name);
        public IEnumerable<PostDTO> GetPostsBySearch(string title);
        public void DeletePost(int id);
        public void DeleteUsers(int id);
        public void ModifyUsers(int id, string name);
        public void ModerateComments(int id);
        public void ManageSiteSettings();
        public void PostContent();
        public void DeleteOwnPost(int postid, int userid);
        public void CommentOnPost();
        public void VoteOnPost();
        public void FavouritePost(int postId, int userId);
    }
}
