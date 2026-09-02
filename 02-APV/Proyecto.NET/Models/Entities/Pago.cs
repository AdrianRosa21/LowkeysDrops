using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Pago")]
[Index("IdPedido", Name = "UQ_Pago_Pedido", IsUnique = true)]
public partial class Pago
{
    [Key]
    public int IdPago { get; set; }

    public int IdPedido { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Metodo { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [StringLength(120)]
    public string? Referencia { get; set; }

    public DateTime? FechaPago { get; set; }

    [ForeignKey("IdPedido")]
    [InverseProperty("Pago")]
    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
