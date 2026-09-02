using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class ProductoCreateDto
    {
        [Required]
        public int IdDrop { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = null!;
        [StringLength(700)]
        public string? Descripcion { get; set; }
        [StringLength(30)]
        public string? Talla { get; set; }
        [Required]
        [Range(0.01, 1000000)]
        public decimal Precio { get; set; }
        [StringLength(500)]
        public string? ImagenUrl { get; set; }
        public bool EsUnico { get; set; } = true;
        [Range(0, 10000)]
        public int Stock { get; set; } = 1;
    }

    public class ProductoUpdateDto
    {
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = null!;
        [StringLength(700)]
        public string? Descripcion { get; set; }
        [StringLength(30)]
        public string? Talla { get; set; }
        [Required]
        [Range(0.01, 1000000)]
        public decimal Precio { get; set; }
        [StringLength(500)]
        public string? ImagenUrl { get; set; }
        public bool EsUnico { get; set; } = true;
        [Range(0, 10000)]
        public int Stock { get; set; } = 1;
        [Required]
        [StringLength(15)]
        public string Estado { get; set; } = null!;
    }

    public class ProductoResponseDto
    {
        public int IdProducto { get; set; }
        public int IdDrop { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Talla { get; set; }
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public bool EsUnico { get; set; }
        public int Stock { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
