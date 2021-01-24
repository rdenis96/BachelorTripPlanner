using BachelorTripPlanner.Models;
using BusinessLogic.Notifications;
using DataLayer.CompositionRoot;
using Domain.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTripPlanner.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly NotificationsWorker _notificationsWorker;

        public NotificationsController(ICompositionRoot compositionRoot)
        {
            _notificationsWorker = compositionRoot.GetImplementation<NotificationsWorker>();
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