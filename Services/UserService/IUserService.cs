using DocApp.Models;

namespace DocApp.Services.UserService
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(Guid Id);
        void Add(User user);
        void Update(User user);
        void Delete(Guid Id);
        void SaveChanges();
    }
}
