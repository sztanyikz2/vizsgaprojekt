using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vizsgaController.Persistence
{
    public class VizsgaDbContext:DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Admin> admin { get; set; }
        public VizsgaDbContext(DbContextOptions<VizsgaDbContext> options): base(options) { }
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
}
