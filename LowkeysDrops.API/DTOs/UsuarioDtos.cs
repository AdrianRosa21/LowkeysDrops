using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Dui { get; set; }
        public string Rol { get; set; } = null!;
        public bool Estado { get; set; }
    }

    public class RegistroRepartidorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        [StringLength(100)]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, MinimumLength = 6)]
        public string Contrasena { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; } = null!;
    }

    public class ActualizarClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        [StringLength(100)]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El DUI es obligatorio.")]
        [StringLength(20)]
        public string Dui { get; set; } = null!;
    }

    public class CambiarEstadoUsuarioDto
    {
        [Required]
        public bool Activo { get; set; }
    }
}
