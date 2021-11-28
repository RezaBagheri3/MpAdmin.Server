using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Enums;

namespace MpAdmin.Server.Models
{
    public class AddCustomerModel
    {
        public string fullName { get; set; }

        public string phoneNumber { get; set; }

        public string address { get; set; }

        public int customerType { get; set; }
    }
}
