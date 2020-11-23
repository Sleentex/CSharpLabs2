using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Call> Calls { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Call>()
                .HasOne(call => call.Client)
                .WithMany(c => c.Calls)
                .HasForeignKey(call => call.ClientId);

            modelBuilder.Entity<Call>()
                .HasOne(call => call.City)
                .WithMany(c => c.Calls)
                .HasForeignKey(call => call.CityId);
        }
    }
}
