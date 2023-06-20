using DocApp.Data;
using DocApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DocApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DocAppContext _db;

        public UserRepository(DocAppContext context)
        {
            _db = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = _db.Users.ToList();
            return users;
        }

        public User? GetUserById(string id)
        {
            var user = _db.Users.Find(new Guid(id));
            return user;
        }

        public void AddUser(User product)
        {
            _db.Users.AddAsync(product);
            _db.SaveChangesAsync();
        }

        public void UpdateUser(User product)
        {
            _db.Users.Update(product);
            _db.SaveChangesAsync();
        }

        public void DeleteUser(string id)
        {
            var user = _db.Users.Find(new Guid(id));
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChangesAsync();
            }
        }
    }

}
