using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public interface IUserModel
    {
        public void Registration(string name, string password);
        public User ValidateUser(string username, string password);
        public void RoleModify(int userid);
        public void ModifyPassword(string username, string password);
    }
}
