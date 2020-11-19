using Domain.Accounts;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IUserRepository : IBasicRepository<User>
    {
        User GetByEmail(string email);

        User GetByEmailAndPassword(string email, string password);

        List<int> GetIdsByEmail(List<string> emails);

        //User GetByPhone(string phone);

        //ICollection<User> GetByFirstname(string firstname);

        //ICollection<User> GetByLastname(string lastname);

        //ICollection<User> GetByIp(string ip);

        //ICollection<User> GetAllOnlineBetweenDates(DateTime startDate, DateTime endDate);

        //ICollection<User> GetAllRegisteredBetweenDates(DateTime startDate, DateTime endDate);
    }
}