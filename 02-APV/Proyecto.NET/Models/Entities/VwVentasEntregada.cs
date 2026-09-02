using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Keyless]
public partial class VwVentasEntregada
{
    public int IdPedido { get; set; }

    public DateTime FechaPedido { get; set; }

    public DateTime? FechaEntrega { get; set; }

    [StringLength(100)]
    public string Cliente { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CostoEnvio { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Total { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string MetodoPago { get; set; } = null!;

    [StringLength(100)]
    public string Repartidor { get; set; } = null!;
}
