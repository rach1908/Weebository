using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Animerch.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animerch.Models
{    
    public class Transaction
    {
        public int ID { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public User User{ get; set; }

        [Required]
        public Merchandise Merchandise { get; set; }
    }
}
