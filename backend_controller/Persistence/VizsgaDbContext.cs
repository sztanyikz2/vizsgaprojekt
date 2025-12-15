using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vizsgaController.Persistence
{
    public class VizsgaDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<Comment> comments { get; set; }
        public VizsgaDbContext(DbContextOptions<VizsgaDbContext> options) : base(options) { }
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }
        public string username { get; set; }
        public string useremail { get; set; }
        public string userpassword { get; set; }
        public string Role { get; set; } = "User";
    }
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int postID { get; set; }
        public int userID { get; set; }
        public int categoryID { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime deleted_at { get; set; }
        public int upvotes { get; set; }
        public int downvotes { get; set; }
        public List<Comment> comments { get; set; }
        public Category category { get; set; }
    }
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int categoryID { get; set; }
        public string categoryname { get; set; }
        public string categorydescription { get; set; }
    }
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reportID { get; set; }
        public int postID { get; set; }
        public int userID { get; set; }
        [Required]
        public string reportreason { get; set; }
        public DateTime reportcreated_at { get; set; }
        public string reportstatus { get; set; }

    }
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int commentID { get; set; }
        public int postID { get; set; }
        public string commentcontent { get; set; }
        public DateTime commentcreated_at { get; set; }
    }
}
