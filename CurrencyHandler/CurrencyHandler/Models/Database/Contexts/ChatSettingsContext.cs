using CurrencyHandler.Models.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyHandler.Models.Database.Contexts
{
    public class ChatSettingsContext : DbContext
    {
        public DbSet<ChatSettings> ChatSettings { get; set; }

        public ChatSettingsContext(DbContextOptions options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<ChatSettings>()
                .Property(cs => cs.Currency)
                .HasDefaultValue("UAH");

            mb.Entity<ChatSettings>()
                .Property(cs => cs.Percents)
                .HasDefaultValue(100);
        }

    }
}
