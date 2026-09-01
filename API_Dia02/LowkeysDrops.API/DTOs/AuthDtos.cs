using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class RegistroClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(100)]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contrasena { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El DUI es obligatorio para clientes.")]
        [StringLength(20)]
        public string Dui { get; set; } = null!;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public UsuarioResponseDto Usuario { get; set; } = null!;
    }
}
