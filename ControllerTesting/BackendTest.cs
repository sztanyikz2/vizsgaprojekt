
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
            var ex = Assert.Throws<InvalidDataException>(() => _model.GetUserNamesBySearch("NINCSILYEN").ToList());
            Assert.Contains("Nincs ilyen", ex.Message);
        }
        ///////////////////////////////////////

        [Fact]
        public void PostSearch_Valid()
        {
            var result = _model.GetPostsBySearch("future").ToList();
            Assert.NotEmpty(result);
            Assert.All(result, x => Assert.Contains("Future", x.title));
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
            var ex = Assert.Throws<InvalidDataException>(() => _model.GetPostsBySearch("NINCSILYEN").ToList());
            Assert.Contains("Nincs ilyen", ex.Message);
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
        public async Task ModifyUser_NullDTO()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _model.ModifyUsers(null!));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task ModifyUser_IDOutOfRange(int id)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _model.DeleteUsers(id));
        }
        [Fact]
        public async Task ModifyUser_IDNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.DeleteUsers(999999));
        }
        [Fact]
        public async Task ModifyUser_EmptyName()
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
        public async Task ModifyUser_UsernameExists()
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
        ///////////////////////////////////////////


        [Fact]
        public async Task CreatePost_Valid()
        {
            var dto = new PostDTO
            {
                categoryID = 1,
                content = "This is a test post.",
                created_at = DateTime.Now,
                title = "Test Post",
                userID = 1
            };

            var before = _context.Posts.Count();
            await _model.CreatePost(dto);
            var after = _context.Posts.Count();
            Assert.Equal(before + 1, after);
        }

        [Fact]
        public async Task CreatePost_NullDTO()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _model.CreatePost(null!));
        }

        [Fact]
        public async Task CreatePost_InvalidId()
        {
            var dto = new PostDTO
            {
                categoryID = -1,
                content = "This is a test post.",
                created_at = DateTime.Now,
                title = "test",
                userID = -1
            };
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _model.CreatePost(dto));
        }

        [Fact]
        public async Task CreatePost_UserNotFound()
        {
            var dto = new PostDTO
            {
                categoryID = 1,
                content = "This is a test post.",
                created_at = DateTime.Now,
                title = "test",
                userID = 999999999
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.CreatePost(dto));
        }

        [Fact]
        public async Task CreatePost_CategoryNotFound()
        {
            var dto = new PostDTO
            {
                categoryID = 999999999,
                content = "This is a test post.",
                created_at = DateTime.Now,
                title = "test",
                userID = 1
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.CreatePost(dto));
        }

        [Fact]
        public async Task CreatePost_EmptyTitle()
        {
            var dto = new PostDTO
            {
                categoryID = 1,
                content = "This is a test post.",
                created_at = DateTime.Now,
                title = "   ",
                userID = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => _model.CreatePost(dto));
        }

        [Fact]
        public async Task CreatePost_EmptyContent()
        {
            var dto = new PostDTO
            {
                categoryID = 1,
                content = "   ",
                created_at = DateTime.Now,
                title = "test",
                userID = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => _model.CreatePost(dto));
        }
        ///////////////////////////////////////////
        [Fact]
        public async Task DeletePost_Valid()
        {
            var id = _context.Posts.Select(r => r.PostID).First();
            var before = _context.Posts.Count();
            await _model.DeletePost(id);
            var after = _context.Posts.Count();
            Assert.Equal(before - 1, after);
            Assert.False(_context.Posts.Any(r => r.PostID == id));
        }

        [Fact]
        public async Task DeletePost_InvalidId()
        {
                        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _model.DeletePost(-1));
        }

    }
}