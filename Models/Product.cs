using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KlicKitApi.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }            

        [Required]
        public float Price { get; set; }        
        
        public IEnumerable<UserProducts> Users { get; set; }
        
        
    }
}