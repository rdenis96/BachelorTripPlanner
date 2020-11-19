using Domain.Notifications;

namespace BachelorTripPlanner.Models
{
    public class NotificationViewModel
    {
        public bool IsAccepted { get; set; }

        public Notification Notification { get; set; }
    }
}