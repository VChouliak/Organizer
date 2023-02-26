using Microsoft.EntityFrameworkCore;
using Organizer.Core.Models.Entities;

namespace Organizer.Data
{
    public class OrganizerContext : DbContext
    {
        public OrganizerContext()
        {
            Database.MigrateAsync();           
        }

        public DbSet<Friend> Friends { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Settings.Organizer.Default.MySqlConnectionString);
        }        
    }
}