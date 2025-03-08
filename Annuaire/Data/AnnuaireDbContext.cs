using Microsoft.EntityFrameworkCore;
using Annuaire.Models;

namespace Annuaire.Data
{
    public class AnnuaireDbContext : DbContext
    {
        public DbSet<Societe> Societes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<InfoContact> InfoContacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["annuaireDS"].ConnectionString;
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des relations
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Societe)
                .WithMany(s => s.Contacts)
                .HasForeignKey(c => c.SocieteId);

            modelBuilder.Entity<InfoContact>()
                .HasOne(i => i.Contact)
                .WithMany(c => c.Infos)
                .HasForeignKey(i => i.ContactId);
        }
    }
}
