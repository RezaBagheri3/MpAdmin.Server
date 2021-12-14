using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class AddFactorModel
    {
        public int customerId { get; set; }

        public string customerName { get; set; }

        public int customerType { get; set; }

        public int totalQuantity { get; set; }

        public int totalAmount { get; set; }

        public int discount { get; set; }

        public int payableAmount { get; set; }

        public List<AddFactorWallPaperModel> factorWallPapers { get; set; }
    }
}
