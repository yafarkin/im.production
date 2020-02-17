using System.ComponentModel.DataAnnotations;

namespace IM.Production.WebApp.Dtos
{
    public class NewTeamDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 30 символов")]
        public string Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина логина должна быть от 3 до 30 символов")]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}