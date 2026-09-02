using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class ResenaCreateDto
    {
        [Required]
        public int IdCliente { get; set; }
        [Required]
        [Range(1, 5)]
        public byte Calificacion { get; set; }
        [StringLength(800)]
        public string? Comentario { get; set; }
    }

    public class ResenaResponseDto
    {
        public int IdResena { get; set; }
        public int IdProducto { get; set; }
        public int IdCliente { get; set; }
        public byte Calificacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
