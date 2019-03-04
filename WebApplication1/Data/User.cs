using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animerch.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animerch.Data
{
    public class User : IdentityUser
    {
        public ICollection<MerchVsUser> Transactions { get; set; }
    }
}
