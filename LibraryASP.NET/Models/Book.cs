using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LibraryASP.NET.Models
{
    public class Book
    {
        public Guid BookID { get; set; }
        [Required]
        [Display(Name = "Name Book")]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Date Edition")]
        public DateTime DateEdition { get; set; }
        [Required]
        [Display(Name = "Stock")]
        
        public int Stock { get; set; }

        public virtual ICollection<History> Recorders { get; set; }
       

    }
}