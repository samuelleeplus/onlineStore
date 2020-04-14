using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<ImageUri> ImageUris { get; set; }
        public DbSet<Distributor> Distributors { get; set; }

        public DbSet<Product> Products { get; set; }
     
    }
}
