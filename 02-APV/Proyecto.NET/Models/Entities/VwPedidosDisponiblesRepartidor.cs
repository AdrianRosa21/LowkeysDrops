using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Keyless]
public partial class VwPedidosDisponiblesRepartidor
{
    public int IdPedido { get; set; }

    public DateTime FechaPedido { get; set; }

    [StringLength(100)]
    public string Cliente { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string TipoDireccion { get; set; } = null!;

    [StringLength(80)]
    public string Departamento { get; set; } = null!;

    [StringLength(100)]
    public string Municipio { get; set; } = null!;

    [StringLength(300)]
    public string DireccionTexto { get; set; } = null!;

    [StringLength(300)]
    public string? Referencia { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Total { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string MetodoPago { get; set; } = null!;
}
