﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Context
{
    public class OnlineStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }
     
    }
}
