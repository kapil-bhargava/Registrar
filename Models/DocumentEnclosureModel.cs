using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class DocumentEnclosureModel
    {
        public int DocumentEnclosureId { get; set; }

        [Required(ErrorMessage = "Document Name is required.")]
        [StringLength(200)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required(ErrorMessage = "Short Name is required.")]
        [StringLength(50)]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Mandatory")]
        public bool IsMandatory { get; set; }

        [Display(Name = "Original Required")]
        public bool IsOriginalRequired { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}