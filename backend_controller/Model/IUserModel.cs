using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public interface IUserModel
    {
        public Task Registration(string name, string password);
        public User ValidateUser(string username, string password);
        public Task RoleModify(int userid);
        public Task ModifyPassword(string username, string password);
    }
}
