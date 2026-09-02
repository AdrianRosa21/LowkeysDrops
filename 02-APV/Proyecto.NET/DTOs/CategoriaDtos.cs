using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class CategoriaCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;
        [StringLength(300)]
        public string? Descripcion { get; set; }
    }

    public class CategoriaUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;
        [StringLength(300)]
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class CategoriaResponseDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
