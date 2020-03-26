﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int TaxID { get; set; }
        public string Address { get; set; }
    }
}
