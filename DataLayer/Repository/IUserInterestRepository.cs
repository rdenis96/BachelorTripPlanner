using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repository
{
    public interface IUserInterestRepository : IBasicRepository<UserInterest>
    {
        UserInterest GetByUserId(int userId);
    }
}