using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using vizsgaController.Dtos;
using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public class UserModel :IUserModel
    {
        private readonly NewsDbContext _context;
        public UserModel(NewsDbContext context)
        {
            _context = context;
        }
        public async Task Registration(string name, string password)
        {
            if (_context.Users.Any(u => u.Username == name))
            {
                throw new InvalidOperationException("Already exists");
            }
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Add(new User { Username = name, Userpassword = HashPassword(password, "reddit2"), Role = "User" });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public User ValidateUser(string username, string password)
        {
            var hash = HashPassword(password, "reddit2");
            var user = _context.Users.Where(x => x.Username == username);
            return user.Where(x => x.Userpassword == hash).FirstOrDefault();
        }
        private string HashPassword(string password, string salt)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);

        }
        public async Task RoleModify(int userid)
        {
            using var trx= _context.Database.BeginTransaction();
            {
                var user = _context.Users.Where(x => x.UserID == userid).FirstOrDefault();
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }
                user.Role = "Admin";
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public async Task ModifyPassword(string username, string password)
        {
            using var trx=_context.Database.BeginTransaction();
            {
                var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }
                user.Userpassword = HashPassword(password, "reddit2");
                _context.SaveChanges();
                trx.Commit();
            }
        }
    }
}
