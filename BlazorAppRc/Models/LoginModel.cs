using System.ComponentModel.DataAnnotations;

namespace BlazorAppRc.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Password { get; set; }
    }

    public class TokenResponse
    {
        public string? Token { get; set; }
        public string? AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

}
