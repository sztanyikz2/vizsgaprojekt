using Microsoft.EntityFrameworkCore;
using vizsgaController.Dtos;
using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public class NewsModel : INewsModel
    {
        private readonly NewsDbContext _context;
        public NewsModel(NewsDbContext context)
        {
            _context = context;
        }
        public IEnumerable<UserDTO> GetUserNamesBySearch(string name)
        {
            if (name != null)
            {
                return _context.Users.Where(x => x.Username.ToLower() == name.ToLower()).Select(x => new UserDTO
                {
                    username = x.Username,
                    useremail = x.Useremail,
                    userpassword = x.Userpassword,
                });


            }
            throw new InvalidDataException("Töltsd ki a keresőmezőt, pretty please");
        }
        public IEnumerable<PostDTO> GetPostsBySearch(string title)
        {
            if (title != null)
            {
                return _context.Posts.Where(x => x.Title.ToLower().Contains(title.ToLower())).Select(x => new PostDTO
                {
                    title = x.Title,
                    content = x.Content,
                    created_at = x.Created_at,
                    deleted_at = x.Deleted_at,
                });
            }
            throw new InvalidDataException("Töltsd ki a keresőmezőt, pretty please");
        }
        public void DeletePost(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Remove(_context.Posts.Where(x => x.PostID == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void ModerateComments(int id)
        {

            using var trx = _context.Database.BeginTransaction();
            {
                _context.Comments.Remove(_context.Comments.Where(x => x.CommentID == id).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void DeleteOwnPost(int postid, int userid)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Posts.Remove(_context.Posts.Where(x => x.PostID == postid && x.UserID == userid).FirstOrDefault()); ///usert valahogyan használni kellene, összekötni
                _context.Comments.Remove(_context.Comments.Where(x => x.PostID == postid).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void FavouritePost(int postId, int userId)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var post = _context.Posts.Where(x => x.PostID == postId).FirstOrDefault();
                var user = _context.Users.Where(x => x.UserID == userId).FirstOrDefault();
                if (post != null && user!=null)
                {
                    user.Favourites.Add(post);
                    _context.SaveChanges();
                    trx.Commit();
                }
                else
                {
                    throw new InvalidDataException("Post or User not found");
                }
            }

        }
        public void UnfavouritePost(int postId, int userId)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var post = _context.Posts.Where(x => x.PostID == postId).FirstOrDefault();
                var user = _context.Users.Where(x => x.UserID == userId).FirstOrDefault();
                if (post != null && user!=null && user.Favourites.Contains(post))
                {
                    user.Favourites.Remove(post);
                    _context.SaveChanges();
                    trx.Commit();
                }
                else
                {
                    throw new InvalidDataException("Post or User not found");
                }
            }

        }
        public void DeleteUsers(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Remove(_context.Users.Where(x => x.UserID == id).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void ModifyUsers(int id, string name)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var user = _context.Users.Where(x => x.UserID == id).FirstOrDefault();
                if (user != null)
                {
                    user.Username = name ;
                    _context.SaveChanges();
                    trx.Commit();
                }
                else
                {
                    throw new InvalidDataException("User not found");
                }
            }
        }
        public void UpVoteOnPost(int postId, int userid)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var post = _context.Posts.Where(x => x.PostID == postId).FirstOrDefault();
                var user = _context.Users.Where(x => x.UserID == userid).FirstOrDefault();
                if (post != null && user!=null && user.Upvoted_Posts.Contains(post) == false && user.Downvoted_Posts.Contains(post) == false)
                {
                    post.Upvotes ++;
                    user.Upvoted_Posts.Add(post);
                    _context.SaveChanges();
                    trx.Commit();
                }
                else
                {
                    throw new InvalidDataException("Post or User not found");
                }
            }
        }
        public void DownVoteOnPost(int postId, int userid)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var post = _context.Posts.Where(x => x.PostID == postId).FirstOrDefault();
                var user = _context.Users.Where(x => x.UserID == userid).FirstOrDefault();
                if (post != null && user != null && user.Upvoted_Posts.Contains(post) == false && user.Downvoted_Posts.Contains(post) == false)
                {
                    post.Downvotes++;
                    user.Downvoted_Posts.Add(post);
                    _context.SaveChanges();
                    trx.Commit();
                }
                else
                {
                    throw new InvalidDataException("Post or User not found");
                }
            }
        }
        public void CommentOnPost()
        {
        }
       
        public void PostContent()
        {
        }
        public void ManageSiteSettings()
        {
        }
    }
}
