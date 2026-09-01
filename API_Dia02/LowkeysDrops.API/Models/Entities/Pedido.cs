using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Pedido")]
[Index("IdCliente", Name = "IX_Pedido_Cliente")]
[Index("Estado", Name = "IX_Pedido_Estado")]
public partial class Pedido
{
    [Key]
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public int IdDireccion { get; set; }

    public DateTime FechaPedido { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CostoEnvio { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Total { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdPedidoNavigation")]
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    [InverseProperty("IdPedidoNavigation")]
    public virtual Entrega? Entrega { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("Pedidos")]
    public virtual Usuario IdClienteNavigation { get; set; } = null!;

    [ForeignKey("IdDireccion")]
    [InverseProperty("Pedidos")]
    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    [InverseProperty("IdPedidoNavigation")]
    public virtual Pago? Pago { get; set; }
}
