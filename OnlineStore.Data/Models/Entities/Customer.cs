using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    class Customer : BaseUser
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string HomeAddress { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
