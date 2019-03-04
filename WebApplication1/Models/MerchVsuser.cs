using Animerch.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animerch.Models
{
    public class MerchVsUser
    {
        public int ID { get; set; }
        public User User { get; set; }
        public Transaction Transaction { get; set; }
        public double Amount { get; set; }
    }
}
