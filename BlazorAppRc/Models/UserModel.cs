using System.ComponentModel.DataAnnotations;

namespace BlazorAppRc.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
