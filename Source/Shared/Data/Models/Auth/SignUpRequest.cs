using System.ComponentModel.DataAnnotations;

namespace Refactorizando.Shared.Data.Models.Auth
{
    public class SignUpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public string GithubProfile { get; set; }

    }
}