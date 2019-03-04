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

        public string Name { get; set; }

        public string Type { get; set; }

        public string Series { get; set; }
        
        public string Manufacturer { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
