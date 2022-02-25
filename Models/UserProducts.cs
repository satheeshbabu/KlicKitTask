using System;

namespace KlicKitApi.Models
{
    public class UserProducts
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }            
        public User User { get; set; }
        public Guid ProductId { get; set; }      
        public Product Product { get; set; }
        public DateTime RequestTime { get; set; }  = DateTime.Now;           
        public bool IsChecked { get; set; } = false;
        public bool IsApproved { get; set; } = false;
    }
}