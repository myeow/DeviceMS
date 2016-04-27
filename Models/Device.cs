using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace DeviceMS.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public string Processor { get; set; }
        public string Ram { get; set; }
        public string HardDrive { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }

        //many to many Softwares to Devices
        public virtual ICollection<SoftwareToDevice> SoftwaresToDevices { get; set; }
        public virtual ICollection<DeviceToUser> DevicesToUsers { get; set; }
    }

    public class DeviceViewModel
    {
        public int DeviceId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Device Name")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public List<string> Email { get; set; }
        [Display(Name = "Product ID")]
        public string ProductId { get; set; }
        [Display(Name = "Processor")]
        public string Processor { get; set; }
        [Display(Name = "RAM")]
        public string Ram { get; set; }
        [Display(Name = "Hard Drive")]
        public string HardDrive { get; set; }
        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        [Display(Name = "Users")]
        public IEnumerable<SelectListItem> UserList { get; set; }
        [Display(Name = "Software")]
        public List<CheckBoxViewModel> Softwares { get; set; }
        [Display(Name = "Softwares")]
        public List<string> SoftwaresList { get; set; }
    }

}