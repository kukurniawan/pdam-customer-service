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
    }
}