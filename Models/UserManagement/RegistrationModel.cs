using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayout.Models.UserManagement
{
    public class RegistrationModel : RegistrationRoleMapping
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter user name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Display(Name = "Login ID")]
        [Required(ErrorMessage = "Please enter login id")]
        [StringLength(50)]
        public string LoginID { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Please enter mobile")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Display(Name = "Email ID")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email")]
        [StringLength(50)]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public string SaltedHash { get; set; }
        public int IsUserActive { get; set; }
        public string RefID { get; set; }
        public string RefType { get; set; }
        public string IPAddress { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
       
    }
}