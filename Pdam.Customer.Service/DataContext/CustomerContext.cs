using System;
using Microsoft.EntityFrameworkCore;

namespace Pdam.Customer.Service.DataContext
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql($"{Environment.GetEnvironmentVariable("PdamCustomerConnectionString")}");
            }
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<CustomerStatusLog> CustomerStatusLogs { get; set; }
        public DbSet<CustomerAsset> CustomerAssets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(x =>
                x.HasIndex(x => new{ x.CompanyCode, x.CustomerCode}).IsUnique());
            
            modelBuilder.Entity<CustomerAddress>(x =>
                x.HasOne(c => c.Customer)
                    .WithOne(b => b.CustomerAddress)
                    .HasForeignKey<CustomerAddress>(c => c.Id));
            
            modelBuilder.Entity<CustomerContact>(x =>
                x.HasOne(c => c.Customer)
                    .WithMany(b => b.CustomerContacts)
                    .HasForeignKey(c => c.CustomerId));
            
            modelBuilder.Entity<CustomerStatusLog>(x =>
                x.HasOne(c => c.Customer)
                    .WithMany(b => b.CustomerStatusLogs)
                    .HasForeignKey(c => c.CustomerId));
            
            modelBuilder.Entity<Router>(x =>
                x.HasIndex(x => new{ x.CompanyCode, x.RouteCode}).IsUnique());
            
            modelBuilder.Entity<Customer>(x =>
                x.HasOne(c => c.Router)
                    .WithMany(b => b.Customers)
                    .HasForeignKey(c => c.RouterId));
            
            modelBuilder.Entity<CustomerAsset>(x =>
                x.HasOne(c => c.Customer)
                    .WithMany(b => b.CustomerAssets)
                    .HasForeignKey(c => c.CustomerId));
        }
    }
}