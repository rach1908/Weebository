﻿using System;
using System.Collections.Generic;
using System.Text;
using Animerch.Data;
using Animerch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Animerch.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User");
            builder.Entity<User>().HasMany(u => u.Friends);
            builder.Entity<Transaction>().Property(t => t.Amount).HasDefaultValue(1);
            builder.Entity<FriendEntry>().HasKey(k => new { k.UserID, k.FriendID });       
        }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
        public DbSet<FriendEntry> FriendEntry { get; set; }
    }
}
