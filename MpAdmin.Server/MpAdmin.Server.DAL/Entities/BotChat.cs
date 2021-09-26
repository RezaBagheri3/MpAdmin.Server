using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkReportApp.DAL;

namespace MpAdmin.Server.DAL.Entities
{
    public class BotChat : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ChatId { get; set; }
    }
}
