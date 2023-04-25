using Microsoft.EntityFrameworkCore;
using Organizer.Core.Interfaces.Concurrency;
using Organizer.Core.Models.Entities;

namespace Organizer.Infrastructure.Data
{
    public class OrganizerContext : DbContext
    {
        public OrganizerContext()
        {
            Database.MigrateAsync();
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Settings.Organizer.Default.MySqlConnectionString, ServerVersion.AutoDetect(Settings.Organizer.Default.MySqlConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>()
                 .Property(x => x.Version).IsRequired()
                 .IsConcurrencyToken()
                 .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

            modelBuilder.Entity<Meeting>()
                 .Property(x => x.Version).IsRequired()
                 .IsConcurrencyToken()
                 .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

            modelBuilder.Entity<ProgrammingLanguage>()
                 .Property(x => x.Version).IsRequired()
                 .IsConcurrencyToken()
                 .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var concurrencyTokenEntries = ChangeTracker.Entries<IVersionedRow>();
            foreach (var entry in concurrencyTokenEntries)
            {
                if (entry.State == EntityState.Unchanged)
                {
                    continue;
                }
                entry.Entity.Version = Guid.NewGuid();                
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}