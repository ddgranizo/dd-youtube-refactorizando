using System.ComponentModel.DataAnnotations;

namespace Refactorizando.Shared.Data.Models.Auth
{
    public class UserInfo
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}