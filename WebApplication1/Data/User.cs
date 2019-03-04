using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animerch.Models;

namespace Animerch.Data
{
    public class User : IdentityUser
    {
        public List<Transaction> Transactions { get; set; }
    }
}
