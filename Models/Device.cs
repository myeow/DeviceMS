using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeviceMS.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Owner")]
        public string UserId { get; set; }
        //[ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Device Name")]
        public string Brand { get; set; }
        [Display(Name = "Product ID")]
        public string ProductId { get; set; }
        [Display(Name = "Processor")]
        public string Processor { get; set; }
        [Display(Name = "RAM")]
        public string Ram { get; set; }
        [Display(Name = "Hard Drive")]
        public string HardDrive { get; set; }

        [Display(Name = "Software")]
        public int SoftwareId { get; set; }
        [ForeignKey("SoftwareId")]
        public virtual Software Software { get; set;}
        public virtual ICollection<Log> Logs { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateModified { get; set; }
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
    }
}