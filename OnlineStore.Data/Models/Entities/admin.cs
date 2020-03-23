using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class Admin
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string lastName { get; set; }
        public string email_address { get; set; }
        public string password { get; set; }

    }
}
