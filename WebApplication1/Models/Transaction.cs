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
        public decimal Price { get; set; }

        public List<User> Users{ get; set; }

        public ICollection<MerchVsUser> Merchandise { get; set; }
    }
}
