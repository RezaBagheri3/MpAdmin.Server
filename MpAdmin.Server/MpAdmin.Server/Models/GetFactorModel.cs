using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class GetFactorModel
    {
        public int id { get; set; }

        public string customerName { get; set; }

        public int customerType { get; set; }

        public string dateTime { get; set; }

        public int totalQuantity { get; set; }

        public int totalAmount { get; set; }

        public int final { get; set; }

        public int totalProfit { get; set; }

        public int discount { get; set; }

        public int payableAmount { get; set; }

        public int customerId { get; set; }
    }
}
