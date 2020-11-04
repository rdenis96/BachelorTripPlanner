using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface INotificationsRepository : IBasicRepository<Notification>
    {
        IEnumerable<Notification> GetByUserId(int userId);
    }
}