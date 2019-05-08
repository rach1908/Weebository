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

        public ApplicationDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User");
            builder.Entity<Transaction>().Property(t => t.Amount).HasDefaultValue(1);
            //builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });
        }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
    }
}
