//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cis237assignment6.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Beverage
    {
        //[Required] is a data annotation that requires input for the fields
        [Required]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string pack { get; set; }
        [Required]
        public decimal price { get; set; }
        
        public bool active { get; set; }
    }
}
