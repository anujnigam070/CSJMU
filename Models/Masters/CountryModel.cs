using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CountryModel : BaseEntity
    {
        [Key]
        public int CountryId { get; set; }

        [Display(Name ="Country Name")]
        [Required(ErrorMessage = "Please enter country name")]
        [StringLength(50)]
        public string CountryName { get; set; }

        [Display(Name = "Country Status")]
        [Required(ErrorMessage = "Please select status")]
        public string CountryStatus { get; set; }
    }
}
