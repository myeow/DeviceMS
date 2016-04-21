using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMS.Models
{
    public class DeviceToUser
    {
        public int DeviceToUserID { get; set; }
        public int DeviceID { get; set; }
        public int UserID { get; set; }

        //many to many Devices - Users
        public virtual Device Device { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}