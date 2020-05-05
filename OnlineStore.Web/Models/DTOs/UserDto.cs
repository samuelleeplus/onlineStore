using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Models.Entities;

namespace OnlineStore.Web.Models.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string UserId { get; set; }

      //  public Address CustomerAddress { get; set; }

        public IEnumerable<Order> Orders { get; set; }

    }
}
