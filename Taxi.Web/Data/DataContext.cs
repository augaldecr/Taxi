using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TaxiEntity> Taxis { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripDetail> TripDetails { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupDetailEntity> UserGroupDetails { get; set; }
        public DbSet<UserGroupRequestEntity> UserGroupRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxiEntity>()
                .HasIndex(t => t.Plaque)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
