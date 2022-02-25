using System.ComponentModel.DataAnnotations;

namespace KlicKitApi.Dtos
{
    public class UserForRegisterDto
    {
        [Required]    
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }        
    }
}