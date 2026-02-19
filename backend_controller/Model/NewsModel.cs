using System.Xml.Linq;
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
            if (name == null)
            {
                throw new ArgumentException("Töltsd ki a keresőmezőt, pretty please");
            }
            if (!_context.Users.Any(x => x.Username.ToLower().Contains(name.ToLower())))
            {
                throw new InvalidDataException("Nincs ilyen nevű user");
            }
            return _context.Users.Where(x => x.Username.ToLower() == name.ToLower()).Select(x => new UserDTO
            {
                userID = x.UserID,
                username = x.Username,
                useremail = x.Useremail,
                userpassword = x.Userpassword
            });

        }
        public IEnumerable<PostDTO> GetPostsBySearch(string title)
        {
            if (title == null)
            {
                throw new ArgumentException("Töltsd ki a keresőmezőt, pretty please");
            }
            if (!_context.Posts.Any(x => x.Title.ToLower().Contains(title.ToLower())))
            {
                throw new InvalidDataException("Nincs ilyen post");
            }
            return _context.Posts.Where(x => x.Title.ToLower().Contains(title.ToLower())).Select(x => new PostDTO
            {
                title = x.Title,
                content = x.Content,
                created_at = x.Created_at
            });
        }
        public async Task DeleteUsers(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Az id csak pozitív egész lehet.");
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserID == id);
            if (user is null)
                throw new KeyNotFoundException($"Nincs ember a megadott azonosítóval: {id}.");

            using var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Remove(_context.Users.Where(x => x.UserID == id).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public async Task ModifyUsers(ModifyUserDTO dto)
        {
            if (dto is null) throw new ArgumentNullException("DTO nonexistant", nameof(dto));

            if (dto.id <= 0) throw new ArgumentOutOfRangeException("ID can't be 0 or negative", nameof(dto.id));

            var user = _context.Users.Where(x => x.UserID == dto.id).FirstOrDefault();
            if (user == null) throw new KeyNotFoundException("User not found");

            if (string.IsNullOrWhiteSpace(dto.name)) throw new ArgumentException("Username can't be empty");

            var userexists = await _context.Users.AnyAsync(x => x.Username == dto.name);
            if (userexists) throw new InvalidOperationException($"Username exists: '{dto.name}'");

            using var trx = _context.Database.BeginTransaction();
            {
                user.Username = dto.name;
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public async Task CreatePost(PostDTO source)
        {
            if (source is null) throw new ArgumentNullException("DTO nonexistant", nameof(source));

            if (source.userID <= 0 || source.categoryID <= 0) throw new ArgumentOutOfRangeException("ID can't be 0 or negative");

            var user = _context.Users.Where(x => x.UserID == source.userID).FirstOrDefault();
            if (user == null) throw new KeyNotFoundException("User not found");
            var cat = _context.Categories.Where(x => x.CategoryID == source.categoryID).FirstOrDefault();
            if (cat == null) throw new KeyNotFoundException("Category not found");

            if (string.IsNullOrWhiteSpace(source.title)) throw new ArgumentException("Title can't be empty");
            if (string.IsNullOrWhiteSpace(source.content)) throw new ArgumentException("Content can't be empty");

            using var trx = _context.Database.BeginTransaction();
            {
                _context.Posts.Add(new Post
                {
                    UserID = source.userID,
                    CategoryID = source.categoryID,
                    Title = source.title,
                    Content = source.content,
                    Created_at = DateTime.UtcNow
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void DeletePost(int id)
        {
            var post = _context.Posts.Where(x => x.PostID == id).First();
            if (post == null) throw new KeyNotFoundException("Post not found");

            int before = _context.Posts.Count();
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Remove(post);
                _context.SaveChanges();
                trx.Commit();
            }
            int after = _context.Posts.Count();
            if (before - after != 1) throw new InvalidDataException("Post wasn't removed");
        }
        public void DeleteOwnPost(DeleteOwnPostDTO dto)
        {
            var user = _context.Users.Where(x => x.UserID == dto.userId).FirstOrDefault();
            if (user == null) throw new KeyNotFoundException("User not found");
            var post = _context.Posts.Where(x => x.PostID == dto.postid).First();
            if (post == null) throw new KeyNotFoundException("Post not found");

            var delpost = _context.Posts.Where(x => x.PostID == dto.postid && x.UserID == dto.userId).FirstOrDefault();

            int before = _context.Posts.Count();
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Posts.Remove(delpost);
                DeleteComments(dto.postid);
                _context.SaveChanges();
                trx.Commit();
            }
            int after = _context.Posts.Count();
            if (before - after != 1) throw new InvalidDataException("Post wasn't removed");
        }

        public void FavouritePost(FavouritePostDTO favpost)
        {
            var user = _context.Users.Where(x => x.UserID == favpost.userId).FirstOrDefault();
            if (user == null) throw new KeyNotFoundException("User not found");
            var post = _context.Posts.Where(x => x.PostID == favpost.postId).First();
            if (post == null) throw new KeyNotFoundException("Post not found");

            using var trx = _context.Database.BeginTransaction();
            {

                if (favpost.addTo)
                {
                    user.Favourites.Add(post);
                }
                else
                {
                    user.Favourites.Remove(post);
                }
                _context.SaveChanges();
                trx.Commit();

            }

        }

        public void voteOnPost(VoteDTO dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var post = _context.Posts.Where(x => x.PostID == dto.postId).FirstOrDefault();
                var user = _context.Users.Where(x => x.UserID == dto.userId).FirstOrDefault();
                if (post != null && user != null)
                {
                    if (!user.Upvoted_Posts.Contains(post) && !user.Downvoted_Posts.Contains(post))
                    {
                        if (dto.isPositive)
                        {
                            post.Votes++;
                        }
                        else
                        {
                            post.Votes--;
                        }
                        _context.SaveChanges();
                        trx.Commit();
                    }
                    else
                    {
                        throw new InvalidDataException("Post or User not found");
                    }
                }
            }
        }

        public void removeVote(removeVoteDTO dto)
        {
            var post = _context.Posts.Where(x => x.PostID == dto.postId).FirstOrDefault();
            var user = _context.Users.Where(x => x.UserID == dto.userId).FirstOrDefault();
            if (post != null && user != null)
            {
                using (var trx = _context.Database.BeginTransaction())
                {
                    if (user.Upvoted_Posts.Contains(post))
                    {
                        user.Upvoted_Posts.Remove(post);
                        post.Votes--;
                        _context.SaveChanges();
                        trx.Commit();
                    }
                    else if (user.Downvoted_Posts.Contains(post))
                    {
                        user.Downvoted_Posts.Remove(post);
                        post.Votes++;
                        _context.SaveChanges();
                        trx.Commit();
                    }
                    else
                    {
                        throw new InvalidDataException("Post or User not found");
                    }
                }
            }
        }

        public void CommentOnPost(CommentDTO source)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                var post = _context.Posts.Where(x => x.PostID == source.postID).FirstOrDefault();
                if (post == null)
                {
                    throw new InvalidDataException("Post not found");
                }
                _context.Comments.Add(new Comment
                {
                    PostID = source.postID,
                    CommentID = source.commentID,
                    CommentContent = source.commentcoontent,
                    CommentCreated_at = DateTime.UtcNow,
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void DeleteComments(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Comments.Remove(_context.Comments.Where(x => x.CommentID == id).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void CreateCategory(CategoryDTO source)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Categories.Add(new Category
                {
                    CategoryID = source.categoryID,
                    Categoryname = source.categoryname,
                    CategoryDescription = source.categorydescription
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void DeleteCategory(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Categories.Remove(_context.Categories.Where(x => x.CategoryID == id).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void CreateReport(ReportDTO source)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Reports.Add(new Report
                {
                    ReportID = source.reportID,
                    PostID = source.postID,
                    UserID = source.userID,
                    ReportReason = source.reportreason,
                    ReportCreated_at = DateTime.UtcNow,
                    ReportStatus = source.reportstatus
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void ManageSiteSettings()
        {
        }
    }
}
