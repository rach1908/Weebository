using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Animerch.Models
{
    public class Merchandise
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Series { get; set; }
        
        [Required]
        public string Manufacturer { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
