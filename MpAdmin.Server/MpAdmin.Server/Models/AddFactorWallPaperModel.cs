using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class AddFactorWallPaperModel
    {
        public string wallPaperCode { get; set; }

        public string batchNumber { get; set; }

        public int quantity { get; set; }

        public int buyPrice { get; set; }

        public int salePrice { get; set; }

        public int totalPrice { get; set; }

        public int profit { get; set; }
    }
}
