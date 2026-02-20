namespace vizsgaController.Persistence
{
    public static class DbSeeder
    {
        public static void Seed(NewsDbContext db)
        {
            // If there are already users, don't seed again
            if (db.Users.Any()) return;

            // --------------------
            // CATEGORIES
            // --------------------
            var categories = new List<Category>
        {
            new Category { Categoryname = "Technology", CategoryDescription = "Tech news and innovations" },
            new Category { Categoryname = "Sports", CategoryDescription = "Sports news and updates" },
            new Category { Categoryname = "Politics", CategoryDescription = "Political news and analysis" },
            new Category { Categoryname = "Entertainment", CategoryDescription = "Movies, music and celebrities" },
            new Category { Categoryname = "Science", CategoryDescription = "Scientific discoveries and research" }
        };

            db.Categories.AddRange(categories);
            db.SaveChanges();

            // --------------------
            // USERS
            // --------------------
            var users = new List<User>
        {
            new User
            {
                Username = "admin",
                Useremail = "admin@news.com",
                Userpassword = "admin123", // ⚠ later use hashed passwords!
                Role = "Admin"
            },
            new User
            {
                Username = "john_doe",
                Useremail = "john@news.com",
                Userpassword = "123456",
                Role = "User"
            },
            new User
            {
                Username = "jane_smith",
                Useremail = "jane@news.com",
                Userpassword = "123456",
                Role = "User"
            }
        };

            db.Users.AddRange(users);
            db.SaveChanges();

            // --------------------
            // POSTS
            // --------------------
            var posts = new List<Post>
        {
            new Post
            {
                Title = "The Future of AI",
                Content = "Artificial Intelligence is transforming the world rapidly...",
                Created_at = DateTime.Now,
                UserID = users[0].UserID,
                CategoryID = categories[0].CategoryID,
                Votes = 15
            },
            new Post
            {
                Title = "Champions League Final Results",
                Content = "Last night's final was full of surprises...",
                Created_at = DateTime.Now,
                UserID = users[1].UserID,
                CategoryID = categories[1].CategoryID,
                Votes = 8
            },
            new Post
            {
                Title = "New Space Telescope Launched",
                Content = "Scientists have successfully launched a new telescope...",
                Created_at = DateTime.Now,
                UserID = users[2].UserID,
                CategoryID = categories[4].CategoryID,
                Votes = 21
            }
        };

            db.Posts.AddRange(posts);
            db.SaveChanges();

            // --------------------
            // COMMENTS
            // --------------------
            var comments = new List<Comment>
        {
            new Comment
            {
                PostID = posts[0].PostID,
                CommentContent = "Very interesting topic!",
                CommentCreated_at = DateTime.Now
            },
            new Comment
            {
                PostID = posts[0].PostID,
                CommentContent = "AI will change everything.",
                CommentCreated_at = DateTime.Now
            },
            new Comment
            {
                PostID = posts[1].PostID,
                CommentContent = "Best match of the year!",
                CommentCreated_at = DateTime.Now
            }
        };

            db.Comments.AddRange(comments);
            db.SaveChanges();

            // --------------------
            // REPORTS
            // --------------------
            var reports = new List<Report>
        {
            new Report
            {
                PostID = posts[1].PostID,
                UserID = users[2].UserID,
                ReportReason = "Inappropriate content",
                ReportCreated_at = DateTime.Now,
                ReportStatus = "Pending"
            }
        };

            db.Reports.AddRange(reports);
            db.SaveChanges();
        }
    }

}
