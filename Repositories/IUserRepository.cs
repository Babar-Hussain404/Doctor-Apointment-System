using DocApp.Models;

namespace DocApp.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(string id);
        void AddUser(User product);
        void UpdateUser(User product);
        void DeleteUser(string id);
    }
}
