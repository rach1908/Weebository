using Animerch.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Animerch.Models
{
    public class FriendEntry
    {     
        [Key]
        public string UserID { get; set; }
        public User User { get; set; }

        [Key]
        public string FriendID { get; set; }        
        public User Friend { get; set; }

        public bool RequestSent { get; set; }
        public bool RequestAccepted { get; set; }
    }
}
