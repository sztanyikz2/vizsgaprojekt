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
            var name = _context.Users
                .Where(r => r.Userpassword == "admin123")
                .Select(r => r.Username)
                .First();

            var dto = _model.GetUserNamesBySearch(name);

            Assert.Equal(name, dto.);
            Assert.Equal("Chernobyl 1986", dto.RoomName);
            Assert.True(dto.Price > 0);
            Assert.False(string.IsNullOrWhiteSpace(dto.CategoryName));
            Assert.False(string.IsNullOrWhiteSpace(dto.LocationName));
        }
    }
}