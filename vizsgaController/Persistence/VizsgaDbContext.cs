using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vizsgaController.Persistence
{
    public class VizsgaDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Admin> admin { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Report> report { get; set; }
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
    }
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int adminID { get; set; }
        public string adminname { get; set; }
        public string adminemail { get; set; }
        public string adminpassword { get; set; }
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

    public class Upvote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int upvoteID { get; set; }
        public int postID { get; set; }
        public int userID { get; set; }
        public DateTime upvotecreated_at { get; set; }

    }

    public class Downvote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int downvoteID { get; set; }
        public int postID { get; set; }
        public int userID { get; set; }
        public DateTime downvotecreated_at { get; set; }

    }
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int commentID { get; set; }
        public int postID { get; set; }
        public string commentcoontent { get; set; }
        public DateTime commentcreated_at { get; set; }
    }
}
