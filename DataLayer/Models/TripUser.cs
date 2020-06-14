﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Models
{
    public class TripUser : IEquatable<TripUser>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TripId { get; set; }
        public bool HasAcceptedInvitation { get; set; }
        public bool IsGroupAdmin { get; set; }
        public UserInterestWrapper Interests { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((TripUser)obj);
        }

        public bool Equals(TripUser obj)
        {
            var user = obj as TripUser;
            return user != null &&
                   Id == user.Id &&
                   UserId == user.UserId &&
                   TripId == user.TripId &&
                   HasAcceptedInvitation == user.HasAcceptedInvitation &&
                   IsGroupAdmin == user.IsGroupAdmin &&
                   EqualityComparer<UserInterestWrapper>.Default.Equals(Interests, user.Interests);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserId, TripId, HasAcceptedInvitation, IsGroupAdmin, Interests);
        }
    }
}