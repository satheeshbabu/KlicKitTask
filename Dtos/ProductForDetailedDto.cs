using System;
using System.Collections.Generic;

namespace KlicKitApi.Dtos
{
    public class ProductForDetailedDto
    {        
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }             
        public float Price { get; set; } 
        public IEnumerable<ProductForDetailedDto> Users { get; set; }     
    }
}