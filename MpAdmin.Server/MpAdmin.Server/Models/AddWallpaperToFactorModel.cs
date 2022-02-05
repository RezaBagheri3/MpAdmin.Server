using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class AddWallpaperToFactorModel
    {
        public string wallPaperCode { get; set; }

        public string batchNumber { get; set; }

        public int quantity { get; set; }

        public int buyPrice { get; set; }

        public int salePrice { get; set; }

        public int factorId { get; set; }
    }
}
