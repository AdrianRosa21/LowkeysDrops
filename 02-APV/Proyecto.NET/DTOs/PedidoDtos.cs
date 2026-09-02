using System.ComponentModel.DataAnnotations;

namespace LowkeysDrops.API.DTOs
{
    public class PedidoCreateDto
    {
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdDireccion { get; set; }
        [Required]
        [StringLength(20)]
        public string MetodoPago { get; set; } = null!;
    }

    public class AgregarProductoPedidoDto
    {
        [Required]
        public int IdProducto { get; set; }
        [Required]
        [Range(1, 100)]
        public int Cantidad { get; set; } = 1;
    }

    public class VerificarPagoDto
    {
        [Required]
        [StringLength(120)]
        public string Referencia { get; set; } = null!;
    }

    public class TomarPedidoDto
    {
        [Required]
        public int IdRepartidor { get; set; }
    }

    public class MarcarEnCaminoDto
    {
        [Required]
        public int IdRepartidor { get; set; }
    }

    public class RegistrarEntregaDto
    {
        [Required]
        public int IdRepartidor { get; set; }
        [StringLength(500)]
        public string? FotoEntregaUrl { get; set; }
        [StringLength(500)]
        public string? Observacion { get; set; }
    }

    public class RegistrarEntregaFallidaDto
    {
        [Required]
        public int IdRepartidor { get; set; }
        [StringLength(500)]
        public string? Observacion { get; set; }
    }

    public class ConfirmarRecepcionDto
    {
        [Required]
        public int IdCliente { get; set; }
    }
}
