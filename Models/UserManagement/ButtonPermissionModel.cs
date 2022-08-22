using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class ButtonPermissionModel :BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Form Name")]
        [Required(ErrorMessage = "Please enter form name")]
        [StringLength(50)]
        public string FormName { get; set; }

        [Display(Name = " Add Button")]
        [Required(ErrorMessage = "Please select add button")]
        public string AddButton { get; set; }

        [Display(Name = " Edit Button")]
        [Required(ErrorMessage = "Please select edit button")]
        public string EditButton { get; set; }

        [Display(Name = " Detail Button")]
        [Required(ErrorMessage = "Please select detail button")]
        public string DetailButton { get; set; }

        [Display(Name = " DeleteButton")]
        [Required(ErrorMessage = "Please select delete button")]
        public string DeleteButton { get; set; }

        [Display(Name = " Download Button")]
        [Required(ErrorMessage = "Please select download button")]
        public string DownloadButton { get; set; }

        [Display(Name = " Search Button")]
        [Required(ErrorMessage = "Please select serach button")]
        public string SearchButton { get; set; }

        [Display(Name = " Upload Button")]
        [Required(ErrorMessage = "Please select upload button")]
        public string UploadButton { get; set; }

        [Display(Name = " Extra Button")]
        [Required(ErrorMessage = "Please select extra button")]
        public string ExtraButton { get; set; }

        [Display(Name = " User")]
        [Required(ErrorMessage = "Please select user")]
        public int UserId { get; set; }

        [Display(Name = " Role")]
        [Required(ErrorMessage = "Please select role")]
        public int RoleId { get; set; }


        [Display(Name = " Status")]
        [Required(ErrorMessage = "Please select status")]
        public string Status { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }
    }
}
