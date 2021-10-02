using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.Models
{
    public class UpdateWallPaperModel
    {
        public int id { get; set; }

        public string code { get; set; }

        public string batchNumber { get; set; }

        public string album { get; set; }

        public int stock { get; set; }

        public int buyPrice { get; set; }
    }
}
