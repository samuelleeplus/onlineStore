using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public interface BaseUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
