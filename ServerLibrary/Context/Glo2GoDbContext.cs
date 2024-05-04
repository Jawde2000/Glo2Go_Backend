using BaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class Glo2GoDbContext(DbContextOptions<Glo2GoDbContext> options) : DbContext(options)
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<Traveler> Travelers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }

        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<TimetableCollaborator> TimetableCollaborators { get; set; }

        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Site and Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Site)       // One Site has many Reviews
                .WithMany(s => s.Reviews)  // Many Reviews belong to one Site
                .HasForeignKey(r => r.ReviewSite);  // ForeignKey in Review that points to Site
        }

    }
}
