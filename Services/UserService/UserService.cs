﻿using DocApp.GenericRepository;
using DocApp.Models;

namespace DocApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _repository;

        public UserService(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> GetAll() 
        { 
            return _repository.GetAll();
        }

        public User GetById(Guid Id) 
        {
            return _repository.GetById(Id);
        }

        public void Add(User user) 
        {
            _repository.Add(user);
        }

        public void Update(User user) 
        {
            _repository.Update(user);
        }

        public void Delete(Guid Id) 
        { 
            _repository.Delete(Id);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

    }
}
