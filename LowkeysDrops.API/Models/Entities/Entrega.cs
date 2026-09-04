using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Entrega")]
[Index("IdRepartidor", "Estado", Name = "IX_Entrega_RepartidorEstado")]
[Index("IdPedido", Name = "UQ_Entrega_Pedido", IsUnique = true)]
public partial class Entrega
{
    [Key]
    public int IdEntrega { get; set; }

    public int IdPedido { get; set; }

    public int IdRepartidor { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    public DateTime FechaTomado { get; set; }

    public DateTime? FechaEntrega { get; set; }

    [StringLength(500)]
    public string? FotoEntregaUrl { get; set; }

    [StringLength(500)]
    public string? Observacion { get; set; }

    public bool ConfirmadoCliente { get; set; }

    [ForeignKey("IdPedido")]
    [InverseProperty("Entrega")]
    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    [ForeignKey("IdRepartidor")]
    [InverseProperty("Entregas")]
    public virtual Usuario IdRepartidorNavigation { get; set; } = null!;
}
