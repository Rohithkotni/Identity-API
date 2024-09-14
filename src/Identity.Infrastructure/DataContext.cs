using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Registration> Registrations
        {
            get;
            set;
        }

        public DbSet<Credential> Credentials
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().HasKey(x => x.CustomerKey);
            modelBuilder.Entity<Credential>().HasKey(x => x.EmailAddressId);
            modelBuilder.Entity<Registration>(entity => { entity.HasIndex(e => e.EmailAddress).IsUnique(); });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken =
          default)
        {
            // Define key generation logic for entities
            var keyGenerators = new Dictionary<Type,
              Action<object>> {
          {
            typeof (Registration), entity => {
              var registration = (Registration) entity;

              // Set CustomerKey if not already set
              if (registration.CustomerKey == 0) {
                registration.CustomerKey = GetNextSequentialCustomerId();
              }
            }
          },
          {
            typeof (Credential),
            entity => {
              var credential = (Credential) entity;

              // Set EmailAddressId if not already set
              if (credential.EmailAddressId == 0) {
                credential.EmailAddressId = GetNextSequentialEmailId();
              }
            }
          }
          // Add other entity types and their respective key setting logic if needed
        };

            // Process entities that need key generation
            foreach (var entry in ChangeTracker.Entries()
              .Where(e => e.State == EntityState.Added && keyGenerators.ContainsKey(e.Entity.GetType())))
            {
                var keyGenerator = keyGenerators[entry.Entity.GetType()];
                keyGenerator(entry.Entity);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private int GetNextSequentialCustomerId()
        {
            // Your logic to get the next sequential ID
            // This could involve querying the database to get the highest existing ID and incrementing it
            return Registrations.Max(u => (int?)u.CustomerKey) + 1 ?? 1;
        }

        private int GetNextSequentialEmailId()
        {
            // Your logic to get the next sequential ID
            // This could involve querying the database to get the highest existing ID and incrementing it
            return Credentials.Max(u => (int?)u.EmailAddressId) + 1 ?? 1;
        }
    }
}