using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CheeryAssessment.Models
{
    [Table("tbl_user")]
    public class UserModel
    {
        [Key, Column(Order =1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string email { get; set; }

        [Display(Name = "Username")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string username { get; set; }

        [Display(Name = "Password")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Mobile No.")]
        [Required]
        public string mobile_no { get; set; }
        
        public DateTime? last_login_date { get; set; }
        
        public DateTime? creation_date { get; set; }
        
        public byte is_temp_password { get; set; }

        public virtual UserBalanceModel UserBalanceModel { get; set; }
    }
}