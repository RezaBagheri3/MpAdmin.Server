using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkReportApp.DAL;

namespace MpAdmin.Server.DAL.Entities
{
    public class FactorWallPaper : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string WallPaperCode { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int BuyPrice { get; set; }

        [Required]
        public int SalePrice { get; set; }

        [Required]
        public int Profit { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        public int FactorId { get; set; }


        [ForeignKey("FactorId")]
        public virtual Factor Factor { get; set; }
    }
}
