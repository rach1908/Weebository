using System;
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
            builder.Entity<Transaction>().Property(t => t.Amount).HasDefaultValue(1);
            builder.Entity<FriendEntry>().HasKey(k => new { k.UserID, k.FriendID });
            builder.Entity<FriendEntry>().HasOne(P => P.User).WithMany(P => P.Friends).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<FriendEntry>().Property(FE => FE.RequestAccepted).HasDefaultValue(false);
            builder.Entity<FriendEntry>().Property(FE => FE.RequestSent).HasDefaultValue(true);
        }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
        public DbSet<FriendEntry> FriendEntry { get; set; }
    }
}
