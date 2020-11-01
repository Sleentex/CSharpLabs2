using Lab2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    class ApplicationContext : DbContext
    {
        private const string Host = "localhost";
        private const string Db = "phone_conversation_lab2";
        private const string User = "root";
        private const string Password = "";

        public DbSet<Client> Clients { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Call> Calls { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"Database={Db};Datasource={Host};User={User};Password={Password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Call>()
                .HasOne(order => order.Client)
                .WithMany(o => o.Calls)
                .HasForeignKey(order => order.ClientId);

            modelBuilder.Entity<Call>()
                .HasOne(order => order.City)
                .WithMany(o => o.Calls)
                .HasForeignKey(order => order.CityId);
        }
    }
}
