using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Enums;
using WorkReportApp.DAL;

namespace MpAdmin.Server.DAL.Entities
{
    public class Factor : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public CustomerType CustomerType { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        [Required]
        public int TotalAmount { get; set; }

        [Required]
        public Final Final { get; set; }

        [Required]
        public int TotalProfit { get; set; }

        public int Discount { get; set; }

        [Required]
        public int PayableAmount { get; set; }

        public int CustomerId { get; set; }


        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual ICollection<FactorWallPaper> FactorWallPapers { get; set; }
    }
}
