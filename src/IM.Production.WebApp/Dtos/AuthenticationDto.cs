using System.ComponentModel.DataAnnotations;

namespace IM.Production.WebApp.Dtos
{
    public class AuthenticationDto
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
