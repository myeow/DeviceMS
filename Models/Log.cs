using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeviceMS.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Device")]
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
        [Display(Name = "Old Owner")]
        public string OldOwner { get; set; }
        [Display(Name = "Current Owner")]
        public string NewOwner { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Created By")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string CreatedBy { get; set; }
    }
}