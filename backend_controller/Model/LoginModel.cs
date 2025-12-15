using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using vizsgaController.Dtos;
using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public class LoginModel
    {
        private readonly VizsgaDbContext _context;
        public LoginModel(VizsgaDbContext context)
        {
            _context = context;
        }

        public void Registration(string name, string password)
        {
            if (_context.users.Any(u => u.username == name))
            {
                throw new InvalidOperationException("Already exists");
            }
            using var trx = _context.Database.BeginTransaction();
            {
                _context.users.Add(new User { username = name, userpassword = HashPassword(password, "reddit2")});
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public User ValidateUser(string username, string password)
        {
            var hash = HashPassword(password, "reddit2");
            var user = _context.users.Where(x => x.username == username);
            return user.Where(x => x.userpassword == hash).FirstOrDefault();
        }
        private string HashPassword(string password, string salt)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);

        }
        

    }
}
