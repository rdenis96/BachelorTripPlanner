using Domain.Notifications;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface INotificationsRepository : IBasicRepository<Notification>
    {
        IEnumerable<Notification> GetByUserId(int userId);
    }
}