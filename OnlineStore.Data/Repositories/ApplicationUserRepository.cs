using Microsoft.EntityFrameworkCore;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _Context { get; }

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _Context = context;
        }
    }
}
