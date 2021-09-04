using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Pdam.Customer.Service.DataContext
{
    public static class Extension
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<CustomerContext>();
            context?.Database.Migrate();
            return builder;
        }
        
        public static IApplicationBuilder SetupInitData(this IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<CustomerContext>();
            if (context == null) return builder;
            InitCustomer(context);
            InitAddress(context);
            InitContact(context);
            InitAssets(context);
            return builder;
        }
        
        private static void InitAssets(CustomerContext context)
        {
            var value = File.ReadAllText("DataContext\\init-assets.json");
            var customerAssets = JsonConvert.DeserializeObject<List<CustomerAsset>>(value);
            if (customerAssets == null) return;

            foreach (var entity in from customerAsset in customerAssets
                let w = context.CustomerAssets.FirstOrDefault(c => c.Id == customerAsset.Id)
                where w == null
                select customerAsset)
            {
                context.CustomerAssets.Add(entity);
            }

            context.SaveChanges();
        }
        
        private static void InitContact(CustomerContext context)
        {
            var value = File.ReadAllText("DataContext\\init-contact.json");
            var customerContacts = JsonConvert.DeserializeObject<List<CustomerContact>>(value);
            if (customerContacts == null) return;

            foreach (var entity in from customerAddress in customerContacts
                let w = context.CustomerContacts.FirstOrDefault(c => c.Id == customerAddress.Id)
                where w == null
                select customerAddress)
            {
                context.CustomerContacts.Add(entity);
            }

            context.SaveChanges();
        }

        private static void InitAddress(CustomerContext context)
        {
            var value = File.ReadAllText("DataContext\\init-address.json");
            var customerAddresses = JsonConvert.DeserializeObject<List<CustomerAddress>>(value);
            if (customerAddresses == null) return;

            foreach (var entity in from customerAddress in customerAddresses
                let w = context.CustomerAddress.FirstOrDefault(c => c.Id == customerAddress.Id)
                where w == null
                select customerAddress)
            {
                context.CustomerAddress.Add(entity);
            }

            context.SaveChanges();
        }
        
        private static void InitCustomer(CustomerContext context)
        {
            var value = File.ReadAllText("DataContext\\init-customer.json");
            var customers = JsonConvert.DeserializeObject<List<Customer>>(value);
            if (customers == null) return;

            foreach (var entity in from customer in customers
                let w = context.Customers.FirstOrDefault(c => c.Id == customer.Id)
                where w == null
                select customer)
            {
                context.Customers.Add(entity);
            }

            context.SaveChanges();
        }
    }
}