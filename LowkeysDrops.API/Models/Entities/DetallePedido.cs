using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("DetallePedido")]
[Index("IdProducto", Name = "IX_Detalle_Producto")]
[Index("IdPedido", "IdProducto", Name = "UQ_Detalle_PedidoProducto", IsUnique = true)]
public partial class DetallePedido
{
    [Key]
    public int IdDetalle { get; set; }

    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecioUnitario { get; set; }

    [ForeignKey("IdPedido")]
    [InverseProperty("DetallePedidos")]
    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    [ForeignKey("IdProducto")]
    [InverseProperty("DetallePedidos")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
