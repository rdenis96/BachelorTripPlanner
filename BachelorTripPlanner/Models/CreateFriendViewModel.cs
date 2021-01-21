using BachelorTripPlanner.Attributes;

namespace BachelorTripPlanner.Models
{
    public class CreateFriendViewModel
    {
        [ValidateUser]
        public int UserId { get; set; }

        public string FriendEmail { get; set; }
    }
}