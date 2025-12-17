using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vizsgaController.Persistence
{
    public class NewsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }
    }
    [Index(nameof(Username), nameof(Useremail), IsUnique =true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Useremail { get; set; }
        public string Userpassword { get; set; }
        public string Role { get; set; } = "User";
    }
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Deleted_at { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public List<Comment> Comments { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
        public bool Favourite { get; set; }
    }
    [Index(nameof(Categoryname), IsUnique =true)]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        public string Categoryname { get; set; }
        public string CategoryDescription { get; set; }
    }
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        [Required]
        public string ReportReason { get; set; }
        public DateTime ReportCreated_at { get; set; }
        public string ReportStatus { get; set; }

    }
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentCreated_at { get; set; }
    }
}
