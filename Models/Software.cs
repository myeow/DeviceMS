using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeviceMS.Models
{
    public class Software
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int SoftwareId { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }

        //many to many Softwares to Devices
        public virtual ICollection<SoftwareToDevice> SoftwaresToDevices { get; set; }
    }

    public class SoftwareViewModel
    {
        public int SoftwareId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Software")]
        public string Name { get; set; }
        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
    }
}