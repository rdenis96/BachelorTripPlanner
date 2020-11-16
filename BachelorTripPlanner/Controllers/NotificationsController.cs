using BachelorTripPlanner.Models;
using BachelorTripPlanner.Workers;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTripPlanner.Controllers
{
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly NotificationsWorker _notificationsWorker;

        public NotificationsController()
        {
            _notificationsWorker = new NotificationsWorker();
        }

        [HttpGet("[action]")]
        public IActionResult GetNotifications(int userId)
        {
            var result = _notificationsWorker.GetByUserId(userId);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult SendNotification([FromBody] Notification notification)
        {
            var result = _notificationsWorker.Create(notification);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult RespondNotification([FromBody] NotificationViewModel notification)
        {
            var result = _notificationsWorker.Respond(notification.IsAccepted, notification.Notification);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult DeleteNotification([FromBody] Notification notification)
        {
            var result = _notificationsWorker.Delete(notification);
            return Ok(result);
        }
    }
}