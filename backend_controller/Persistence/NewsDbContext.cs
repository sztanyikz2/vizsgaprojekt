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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Post -> User relationship (author)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User -> Posts relationships (many-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Favourites)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFavourites"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Upvoted_Posts)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserUpvotes"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Downvoted_Posts)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserDownvotes"));

            // Configure Post -> Category relationship
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Comment -> Post relationship
            modelBuilder.Entity<Comment>()
                .HasOne<Post>()
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Image -> Post relationship
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Post)
                .WithMany()
                .HasForeignKey(i => i.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    [Index(nameof(Username), nameof(Useremail), IsUnique = true)]
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
        public int Votes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
    }
    [Index(nameof(Categoryname), IsUnique = true)]
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }

        public byte[] ImageContent { get; set; }

        // MIME típus (image/png, image/jpeg)
        public string ContentType { get; set; }

        // opcionális: milyen entitáshoz tartozik a kép
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}