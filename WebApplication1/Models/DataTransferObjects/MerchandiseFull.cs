using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animerch.Models.DataTransferObjects
{
    public class MerchandiseFull
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Series { get; set; }

        public string Manufacturer { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}
