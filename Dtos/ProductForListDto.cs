using System;

namespace KlicKitApi.Dtos
{
    public class ProductForListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }            
        public float Price { get; set; }   
    }
}