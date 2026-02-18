
using vizsgaController.Dtos;
using vizsgaController.Model;
using vizsgaController.Persistence;

namespace ControllerTesting
{
    public class BackendTest
    {
        private readonly NewsModel _model;
        private readonly NewsDbContext _context;

        public BackendTest()
        {
            _context = DbContextFactory.Create();
            _model = new NewsModel(_context);
        }

        [Fact]
        public void NameSearch_Valid()
        {
            var result = _model.GetUserNamesBySearch("admin").ToList();
            Assert.NotEmpty(result);
            Assert.All(result, x => Assert.Contains("admin", x.username));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NameSearch_EmptyParam(string? name)
        {
            var ex = Assert.Throws<ArgumentException>(() => _model.GetUserNamesBySearch(name!).ToList());
            Assert.Contains("pretty", ex.Message);
        }
        [Fact]
        public void NameSearch_NoMatch()
        {
            var ex = Assert.Throws<KeyNotFoundException>(() => _model.GetUserNamesBySearch("NINCSILYEN").ToList());
            Assert.Contains("nincs ilyen", ex.Message);
        }
        ///////////////////////////////////////
        
        [Fact]
        public void PostSearch_Valid()
        {
            var result = _model.GetPostsBySearch("future").ToList();
            Assert.NotEmpty(result);
            Assert.All(result, x => Assert.Contains("future", x.title));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void PostSearch_EmptyParam(string? title)
        {
            var ex = Assert.Throws<ArgumentException>(() => _model.GetPostsBySearch(title!).ToList());
            Assert.Contains("pretty", ex.Message);
        }
        [Fact]
        public void PostSearch_NoMatch()
        {
            var ex = Assert.Throws<KeyNotFoundException>(() => _model.GetPostsBySearch("NINCSILYEN").ToList());
            Assert.Contains("nincs ilyen", ex.Message);
        }
        ///////////////////////////////////////
        
        [Fact]
        public async Task DeleteUser_Valid()
        {
            var id = _context.Users
                .Where(r => r.Username == "john_doe")
                .Select(r => r.UserID)
                .First();

            var before = _context.Users.Count();

            await _model.DeleteUsers(id);

            var after = _context.Users.Count();
            Assert.Equal(before - 1, after);
            Assert.False(_context.Users.Any(r => r.UserID == id));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteUsers_IDOutOfRange(int id)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _model.DeleteUsers(id));
        }
        [Fact]
        public async Task DeleteUsers_IDNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.DeleteUsers(999999));
        }
        //////////////////////////////////////////
        
        [Fact]
        public async Task ModifyUser_Valid()
        {
            var id = _context.Users
                .Where(r => r.Username == "admin")
                .Select(r => r.UserID)
                .First();

            var dto = new ModifyUserDTO
            {
                id = id,
                name = "adminreal"
            };

            await _model.ModifyUsers(dto);

            var updated = _context.Users.First(r => r.UserID == id);
            Assert.Equal(dto.name, updated.Username);
        }

        [Fact]
        public async Task ModifyRoom_NullDTO()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _model.ModifyUsers(null!));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task ModifyRoom_IDOutOfRange(int id)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _model.DeleteUsers(id));
        }
        [Fact]
        public async Task ModifyRoom_IDNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.DeleteUsers(999999));
        }
        [Fact]
        public async Task ModifyRoom_EmptyName()
        {
            var id = _context.Users.Select(r => r.UserID).First();

            var dto = new ModifyUserDTO
            {
                id = id,
                name = "   "
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _model.ModifyUsers(dto));
        }

        [Fact]
        public async Task ModifyRoom_UsernameExists()
        {
            var id = _context.Users.Select(r => r.UserID).First();

            var dto = new ModifyUserDTO
            {
                id = id,
                name = "jane_smith"
            };
            var before = _context.Users.Count();
            await _model.DeleteUsers(id);

            var after = _context.Users.Count();
            Assert.Equal(before - 1, after);
            Assert.False(_context.Users.Any(r => r.UserID == id));
        }

    }
}