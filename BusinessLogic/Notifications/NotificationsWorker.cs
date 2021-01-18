using BusinessLogic.Accounts;
using Domain.Notifications;
using Domain.Notifications.Enums;
using Domain.Repository;
using System.Collections.Generic;

namespace BusinessLogic.Notifications
{
    public class NotificationsWorker
    {
        private readonly INotificationsRepository _notificationsRepository;
        private readonly ITripsUsersRepository _tripsUsersRepository;
        private readonly FriendsWorker _friendsWorker;

        public NotificationsWorker(INotificationsRepository notificationsRepository,
                    ITripsUsersRepository tripsUsersRepository,
                    FriendsWorker friendsWorker)
        {
            _notificationsRepository = notificationsRepository;
            _tripsUsersRepository = tripsUsersRepository;
            _friendsWorker = friendsWorker;
        }

        public Notification Create(Notification obj)
        {
            var result = _notificationsRepository.Create(obj);
            return result;
        }

        public Notification Respond(bool isAccepted, Notification obj)
        {
            switch (obj.Type)
            {
                case NotificationType.TripInvitation:
                    if (isAccepted)
                    {
                        var tripUser = _tripsUsersRepository.GetByUserIdAndTripId(obj.UserId, obj.TripId.GetValueOrDefault());
                        if (tripUser != null)
                        {
                            tripUser.HasAcceptedInvitation = true;
                            _tripsUsersRepository.Update(tripUser);
                        }
                    }
                    break;

                case NotificationType.FriendRequest:
                    if (isAccepted)
                    {
                        //Create friend for sender person
                        var senderFriendship = _friendsWorker.Create(obj.SenderId.Value, obj.UserId);
                        if (senderFriendship != null)
                        {
                            //Create friend for invited person
                            _ = _friendsWorker.Create(obj.UserId, obj.SenderId.Value);
                        }
                    }
                    break;
            }

            _notificationsRepository.Delete(obj);
            return obj;
        }

        public bool Delete(Notification obj)
        {
            var result = _notificationsRepository.Delete(obj);
            return result;
        }

        public ICollection<Notification> GetAll()
        {
            var result = _notificationsRepository.GetAll();
            return result;
        }

        public IEnumerable<Notification> GetByUserId(int userId)
        {
            var result = _notificationsRepository.GetByUserId(userId);
            return result;
        }

        public Notification GetById(int id)
        {
            var result = _notificationsRepository.GetById(id);
            return result;
        }

        public Notification Update(Notification obj)
        {
            var result = _notificationsRepository.Update(obj);
            return result;
        }
    }
}