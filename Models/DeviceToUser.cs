using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeviceMS.Models
{
    public class DeviceToUser
    {
        public int DeviceToUserId { get; set; }
        //[Key]
        //[Column(Order = 1)]
        public int DeviceID { get; set; }
        //[Key]
        //[Column(Order = 2)]
        public string UserID { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<ApplicationUser> DTUUsers { get; set; }
    }

}