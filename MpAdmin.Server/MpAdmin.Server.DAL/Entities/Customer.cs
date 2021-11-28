using MpAdmin.Server.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkReportApp.DAL;

namespace MpAdmin.Server.DAL.Entities
{
    public class Customer : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required]
        public CustomerType CustomerType { get; set; }



        public virtual ICollection<Factor> Factors { get; set; }
    }
}
