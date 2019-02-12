using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repository
{
    public interface IUserRepository : IBasicRepository<User>
    {
        User GetByEmail(string email);

        //User GetByPhone(string phone);

        //ICollection<User> GetByFirstname(string firstname);

        //ICollection<User> GetByLastname(string lastname);

        //ICollection<User> GetByIp(string ip);

        //ICollection<User> GetAllOnlineBetweenDates(DateTime startDate, DateTime endDate);

        //ICollection<User> GetAllRegisteredBetweenDates(DateTime startDate, DateTime endDate);
    }
}