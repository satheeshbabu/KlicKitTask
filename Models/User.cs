using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KlicKitApi.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } 
        [Required]
        [StringLength(50)]   
        public string Username { get; set; }                
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }                                
        public IEnumerable<UserProducts> Products { get; set; }        
        
    }
}