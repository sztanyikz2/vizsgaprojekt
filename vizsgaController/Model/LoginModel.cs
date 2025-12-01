using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using vizsgaController.Dtos;
using vizsgaController.Persistence;
using static vizsgaController.Dtos.DTOs;

namespace vizsgaController.Model
{
    public class LoginModel
    {
        private readonly VizsgaDbContext _context;
        public LoginModel(VizsgaDbContext context)
        {
            _context = context;
        }

        public void Registration(string name, string password, string role = "User")
        {
            if (_context.users.Any(u => u.username == name))
            {
                throw new InvalidOperationException("Already exists");
            }
            using var trx = _context.Database.BeginTransaction();
            {
                _context.users.Add(new User { username = name, userpassword = HashPassword(password), Role = role });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public User ValidateUser(string username, string password)
        {
            var hash = HashPassword(password);
            var user = _context.users.Where(x => x.username == username);
            return user.Where(x => x.userpassword == hash).FirstOrDefault();
        }
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);

        }
        public DTO.UserDto ShowUsers(int id)
        {
            return _context.users.Where(x => x.userID == id).Select(x => new UserDto
            {
                Name = x.username,
                Password = x.userpassword
            }).First();
        }

    }
}
