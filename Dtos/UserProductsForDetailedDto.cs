using System;

namespace KlicKitApi.Dtos
{
    public class UserProductsForDetailedDto
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }            
        public UserForDetailedDto User { get; set; }
        public Guid ProductId { get; set; }      
        public ProductForDetailedDto Product { get; set; }
        public DateTime RequestTime { get; set; }         
        public bool IsChecked { get; set; } 
        public bool IsApproved { get; set; }
    }
        
}