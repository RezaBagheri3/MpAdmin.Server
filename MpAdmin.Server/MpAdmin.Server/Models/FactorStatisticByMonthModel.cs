using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class FactorStatisticByMonthModel
    {
        public int CustomerFactorsCount { get; set; }

        public int StoreFactorsCount { get; set; }

        public int CustomerTotalQuantity { get; set; }

        public int StoreTotalQuantity { get; set; }

        public int CustomerTotalProfit { get; set; }

        public int StoreTotalProfit { get; set; }
    }
}
