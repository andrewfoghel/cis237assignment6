using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cis237assignment6.Models
{
    public class Drinks
    {
        [Required]
        [DataType(DataType.Custom)]
        [Display(Name = "id")]
        public long id { get; set; }

        [DataType(DataType.Custom)]
        [Display(Name = "name")]
        public string Name { get; set; }

        [DataType(DataType.Custom)]
        [Display(Name = "pack")]
        public string Pack { get; set; }

        [DataType(DataType.Custom)]
        [Display(Name = "price")]
        public int Price { get; set; }

    }
}