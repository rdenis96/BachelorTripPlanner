using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Definitions
{
    static internal class UserDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(k => k.Id);
            modelBuilder.Entity<User>().Property(p => p.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().Property(p => p.Password).IsRequired();
        }
    }
}