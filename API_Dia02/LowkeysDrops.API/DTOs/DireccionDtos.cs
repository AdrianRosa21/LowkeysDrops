using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class DireccionCreateDto
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        [StringLength(10)]
        public string Tipo { get; set; } = null!; // CASA, TRABAJO
        [Required]
        [StringLength(80)]
        public string Departamento { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Municipio { get; set; } = null!;
        [Required]
        [StringLength(300)]
        public string DireccionTexto { get; set; } = null!;
        [StringLength(300)]
        public string? Referencia { get; set; }
    }

    public class DireccionUpdateDto
    {
        [Required]
        [StringLength(10)]
        public string Tipo { get; set; } = null!;
        [Required]
        [StringLength(80)]
        public string Departamento { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Municipio { get; set; } = null!;
        [Required]
        [StringLength(300)]
        public string DireccionTexto { get; set; } = null!;
        [StringLength(300)]
        public string? Referencia { get; set; }
    }

    public class DireccionResponseDto
    {
        public int IdDireccion { get; set; }
        public int IdUsuario { get; set; }
        public string Tipo { get; set; } = null!;
        public string Departamento { get; set; } = null!;
        public string Municipio { get; set; } = null!;
        public string DireccionTexto { get; set; } = null!;
        public string? Referencia { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
