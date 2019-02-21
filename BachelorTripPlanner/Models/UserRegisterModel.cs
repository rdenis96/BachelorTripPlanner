using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Models
{
    public class UserRegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? LastOnline { get; set; }
        public string Ip { get; set; }
        public string Phone { get; set; }
    }
}