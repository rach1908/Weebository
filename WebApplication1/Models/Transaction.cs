﻿using System;
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

        public User User{ get; set; }

        [Required]
        public string UserId { get; set; }

        public Merchandise Merchandise { get; set; }

        [Required]
        public int MerchandiseId { get; set; }
    }
}
