using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class DropCreateDto
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = null!;
        [StringLength(500)]
        public string? Descripcion { get; set; }
        public DateOnly? FechaPublicacion { get; set; }
    }

    public class DropUpdateDto
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = null!;
        [StringLength(500)]
        public string? Descripcion { get; set; }
        public DateOnly? FechaPublicacion { get; set; }
        [Required]
        [StringLength(15)]
        public string Estado { get; set; } = null!;
    }

    public class DropResponseDto
    {
        public int IdDrop { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateOnly? FechaPublicacion { get; set; }
        public string Estado { get; set; } = null!;
    }
}
