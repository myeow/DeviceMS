using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMS.Models
{
    public class SoftwareToDevice
    {
        public int SoftwareToDeviceId { get; set; }
        public int DeviceId { get; set; }
        public int SoftwareId { get; set; }

        //many to many 
        public virtual Device Devices { get; set; }
        public virtual Software Softwares { get; set; }

    }
}