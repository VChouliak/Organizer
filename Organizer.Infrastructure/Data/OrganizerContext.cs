using Microsoft.EntityFrameworkCore;
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Settings.Organizer.Default.MySqlConnectionString);
        }        
    }
}