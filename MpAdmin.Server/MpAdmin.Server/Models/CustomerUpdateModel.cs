using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class CustomerUpdateModel
    {
        public int id { get; set; }

        public string fullName { get; set; }

        public string phoneNumber { get; set; }

        public string address { get; set; }

        public int customerType { get; set; }
    }
}
