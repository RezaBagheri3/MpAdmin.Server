﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpAdmin.Server.DAL.Entities
{
    public class WallPaper
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string BatchNumber { get; set; }

        public string Album { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
