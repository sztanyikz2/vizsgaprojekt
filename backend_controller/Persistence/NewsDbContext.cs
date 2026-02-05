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
        public DbSet<Image> Images { get; set; }
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
        public ICollection<Post> Favourites { get; set; }
        public ICollection<Post> Upvoted_Posts { get; set; }
        public ICollection<Post> Downvoted_Posts { get; set; }
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
        //public int Upvotes { get; set; }
        //public int Downvotes { get; set; }
        public int Votes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
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
    public class Image
    {
        public int ImageId { get; set; }

        public byte[] ImageContent { get; set; }

        // MIME típus (image/png, image/jpeg)
        public string ContentType { get; set; }

        // opcionális: milyen entitáshoz tartozik a kép
        public int PostId { get; set; }
        public Post Post  { get; set; }
    }
}
