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
        [DisplayFormat(DataFormatString = "{0:C00}")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Amount { get; set; }

        public User User{ get; set; }

        //UserId is not required since it is not passed into methods, and as such cannot be verified by the ModelState
        public string UserId { get; set; }

        public Merchandise Merchandise { get; set; }

        [Required]
        public int MerchandiseId { get; set; }
    }
}
