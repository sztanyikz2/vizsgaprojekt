using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using vizsgaController.Dtos;
using vizsgaController.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            if (name == null) throw new ArgumentException("Töltsd ki a keresőmezőt, pretty please");
            if (!_context.Users.Any(x => x.Username.ToLower().Contains(name.ToLower()))) throw new InvalidDataException("Nincs ilyen nevű user");

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
            if (title == null) throw new ArgumentException("Töltsd ki a keresőmezőt, pretty please");
            if (!_context.Posts.Any(x => x.Title.ToLower().Contains(title.ToLower()))) throw new InvalidDataException("Nincs ilyen post");

            return _context.Posts.Where(x => x.Title.ToLower().Contains(title.ToLower())).Select(x => new PostDTO
            {
                title = x.Title,
                content = x.Content,
                created_at = x.Created_at
            });
        }
        public async Task DeleteUsers(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Az id csak pozitív egész lehet.");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);
            if (user is null) throw new KeyNotFoundException($"Nincs ember a megadott azonosítóval: {id}.");

            int before = _context.Users.Count();

            using var trx = _context.Database.BeginTransaction();
            _context.Users.Remove(user);
            _context.SaveChanges();
            trx.Commit();

            int after = _context.Users.Count();
            if (before - after != 1) throw new InvalidOperationException("User wasn't removed");

            await Task.CompletedTask;
        }
        public async Task ModifyUsers(ModifyUserDTO dto)
        {
            if (dto is null) throw new ArgumentNullException("DTO nonexistant", nameof(dto));

            if (dto.id <= 0) throw new ArgumentOutOfRangeException("ID can't be 0 or negative", nameof(dto.id));

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == dto.id);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (string.IsNullOrWhiteSpace(dto.name)) throw new ArgumentException("Username can't be empty");

            var userexists = await _context.Users.AnyAsync(x => x.Username == dto.name);
            if (userexists) throw new InvalidOperationException($"Username exists: '{dto.name}'");

            using var trx = _context.Database.BeginTransaction();
            user.Username = dto.name;
            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public async Task CreatePost(PostDTO source)
        {
            if (source is null) throw new ArgumentNullException("DTO nonexistant", nameof(source));

            if (source.userID <= 0 || source.categoryID <= 0) throw new ArgumentOutOfRangeException("ID can't be 0 or negative");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == source.userID);
            if (user == null) throw new KeyNotFoundException("User not found");
            var cat = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == source.categoryID);
            if (cat == null) throw new KeyNotFoundException("Category not found");

            if (string.IsNullOrWhiteSpace(source.title)) throw new ArgumentException("Title can't be empty");
            if (string.IsNullOrWhiteSpace(source.content)) throw new ArgumentException("Content can't be empty");

            int before = _context.Users.Count();

            using var trx = _context.Database.BeginTransaction();
            _context.Posts.Add(new Post
            {
                UserID = source.userID,
                CategoryID = source.categoryID,
                Title = source.title,
                Content = source.content,
                Created_at = DateTime.UtcNow
            });

            int after = _context.Users.Count();
            if (after - before != 1) throw new InvalidOperationException("User wasn't created");

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public async Task DeletePost(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == id);
            if (post == null) throw new KeyNotFoundException("Post not found");

            int before = _context.Posts.Count();

            using var trx = _context.Database.BeginTransaction();
            {
                _context.Remove(post);
                _context.SaveChanges();
                trx.Commit();
            }

            int after = _context.Posts.Count();
            if (before - after != 1) throw new InvalidOperationException("Post wasn't removed");

            await Task.CompletedTask;
        }
        public async Task DeleteOwnPost(DeleteOwnPostDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == dto.userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == dto.postid);
            if (post == null) throw new KeyNotFoundException("Post not found");

            var delpost = _context.Posts.FirstOrDefault(x => x.PostID == dto.postid && x.UserID == dto.userId);

            int before = _context.Posts.Count();

            using var trx = _context.Database.BeginTransaction();
            {
                _context.Posts.Remove(delpost);
                DeleteComments(dto.postid);
                _context.SaveChanges();
                trx.Commit();
            }

            int after = _context.Posts.Count();
            if (before - after != 1) throw new InvalidOperationException("Post wasn't removed");

            await Task.CompletedTask;
        }

        public async Task FavouritePost(FavouritePostDTO favpost)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == favpost.userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == favpost.postId);
            if (post == null) throw new KeyNotFoundException("Post not found");

            bool alreadyFavourited = user.Favourites.Contains(post);

            using var trx = _context.Database.BeginTransaction();

            if (!alreadyFavourited)
            {
                user.Favourites.Add(post);
            }
            else
            {
                user.Favourites.Remove(post);
            }

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;

        }

        public async Task voteOnPost(VoteDTO dto) //when implementing, upvote will have isUpvote true while downvote will have isUpvote false
        {
            var user = await _context.Users.Include(x => x.Upvoted_Posts).Include(x => x.Downvoted_Posts).FirstOrDefaultAsync(x => x.UserID == dto.userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == dto.postId);
            if (post == null) throw new KeyNotFoundException("Post not found");

            bool alreadyUpvoted = user.Upvoted_Posts.Contains(post);
            bool alreadyDownvoted = user.Downvoted_Posts.Contains(post);

            int beforeupvotes = user.Upvoted_Posts.Count();
            int beforedownvotes = user.Downvoted_Posts.Count();

            using var trx = _context.Database.BeginTransaction();

            if (dto.isUpvote) //we pressed upvote
            {
                if (alreadyUpvoted) //it was already upvoted
                {
                    user.Upvoted_Posts.Remove(post);
                    post.Votes--;
                }
                else
                {
                    if (alreadyDownvoted) //it was already downvoted
                    {
                        user.Downvoted_Posts.Remove(post);
                        post.Votes++;
                        user.Upvoted_Posts.Add(post);
                        _context.SaveChanges();
                        trx.Commit();
                        return;
                    }
                    //it was not voted
                    user.Upvoted_Posts.Add(post);
                    post.Votes++;
                }
            }
            else //we pressed downvote
            {
                if (alreadyDownvoted) //it was already downvoted
                {
                    user.Downvoted_Posts.Remove(post);
                    post.Votes++;
                }
                else
                {
                    if (alreadyUpvoted) //it was already upvoted
                    {
                        user.Upvoted_Posts.Remove(post);
                        post.Votes--;
                        user.Downvoted_Posts.Add(post);
                        _context.SaveChanges();
                        trx.Commit();
                        return;
                    }
                    //it was not voted
                    user.Downvoted_Posts.Add(post);
                    post.Votes--;
                }
            }

            int afterupvotes = user.Upvoted_Posts.Count();
            int afterdownvotes = user.Downvoted_Posts.Count();
            if (beforeupvotes == afterupvotes || beforedownvotes == afterdownvotes) throw new InvalidOperationException("Vote wasn't added/removed");

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }

        public async Task CommentOnPost(CommentDTO source) //could add own comment list to user later
        {
            var post = await _context.Posts.Include(x => x.Comments).FirstOrDefaultAsync(x => x.PostID == source.postID);
            if (post == null) throw new KeyNotFoundException("Post not found");
            if (string.IsNullOrWhiteSpace(source.commentcontent)) throw new ArgumentException("Comment content can't be empty");

            int before = _context.Comments.Count();

            using var trx = _context.Database.BeginTransaction();
            
            _context.Comments.Add(new Comment
            {
                PostID = source.postID,
                CommentContent = source.commentcontent,
                CommentCreated_at = DateTime.UtcNow,
            });

            int after = _context.Comments.Count();
            if (after - before != 1) throw new InvalidOperationException("Comment wasn't added");

            post.Comments.Add(_context.Comments.OrderByDescending(x => x.CommentID).First());

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public async Task DeleteComments(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.CommentID == id);
            if (comment == null) throw new KeyNotFoundException("Comment not found");

            int before = _context.Comments.Count();

            using var trx = _context.Database.BeginTransaction();
            _context.Comments.Remove(comment);

            int after = _context.Comments.Count();
            if (before - after != 1) throw new InvalidOperationException("Comment wasn't removed");

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public async Task CreateCategory(CategoryDTO source)
        {
            if (source is null) throw new ArgumentNullException("DTO nonexistant", nameof(source));

            if (string.IsNullOrWhiteSpace(source.categoryname)) throw new ArgumentException("Name can't be empty");
            if (string.IsNullOrWhiteSpace(source.categorydescription)) throw new ArgumentException("Description can't be empty");

            int before = _context.Categories.Count();

            using var trx = _context.Database.BeginTransaction();
            _context.Categories.Add(new Category
            {
                Categoryname = source.categoryname,
                CategoryDescription = source.categorydescription
            });

            int after = _context.Categories.Count();
            if (after - before != 1) throw new InvalidOperationException("Category wasn't created");

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
            if (category == null) throw new KeyNotFoundException("Category not found");

            int before = _context.Categories.Count();

            using var trx = _context.Database.BeginTransaction();
            _context.Categories.Remove(category);

            int after = _context.Categories.Count();
            if (before - after != 1) throw new InvalidOperationException("Category wasn't removed");

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public async Task CreateReport(ReportDTO source)
        {
            if (source is null) throw new ArgumentNullException("DTO nonexistant", nameof(source));

            if (source.userID <= 0 || source.postID <= 0) throw new ArgumentOutOfRangeException("ID can't be 0 or negative");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == source.userID);
            if (user == null) throw new KeyNotFoundException("User not found");
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostID == source.postID);
            if (post == null) throw new KeyNotFoundException("Post not found");

            if (string.IsNullOrWhiteSpace(source.reportreason)) throw new ArgumentException("Reason can't be empty");

            int before = _context.Reports.Count();

            using var trx = _context.Database.BeginTransaction();
            _context.Reports.Add(new Report
            {
                PostID = source.postID,
                UserID = source.userID,
                ReportReason = source.reportreason,
                ReportCreated_at = DateTime.UtcNow
            });

            int after = _context.Reports.Count();
            if (after - before != 1) throw new InvalidOperationException("Report wasn't created");

            _context.SaveChanges();
            trx.Commit();

            await Task.CompletedTask;
        }
        public void ManageSiteSettings()
        {
        }
    }
}
