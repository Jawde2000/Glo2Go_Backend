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
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<Traveler> Travelers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }

    }
}
