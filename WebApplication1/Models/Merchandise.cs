using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Animerch.Models
{
    public class Merchandise
    {
        public string Name { get; set; }

        public string Type { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
