using Domain.Accounts;
using Domain.Repository;
using System.Collections.Generic;

namespace BusinessLogic.Accounts
{
    public class UserWorker
    {
        private IUserRepository _userRepository;

        public UserWorker(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(User obj)
        {
            return _userRepository.Create(obj);
        }

        public ICollection<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            return _userRepository.GetByEmailAndPassword(email, password);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public bool Delete(User obj)
        {
            return _userRepository.Delete(obj);
        }

        public User Update(User obj)
        {
            return _userRepository.Update(obj);
        }
    }
}