using System.ComponentModel.DataAnnotations;

namespace OVD.API.Dtos
{
    public class AdminForRegisterDto
    {
        [Required]
        public string Username { get; set; }   
        [Required]
        public string Password { get; set; }
    }
}