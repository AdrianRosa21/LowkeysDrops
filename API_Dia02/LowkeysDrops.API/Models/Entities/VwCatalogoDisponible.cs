using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Keyless]
public partial class VwCatalogoDisponible
{
    public int IdProducto { get; set; }

    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [StringLength(700)]
    public string? Descripcion { get; set; }

    [StringLength(30)]
    public string? Talla { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [StringLength(500)]
    public string? ImagenUrl { get; set; }

    public bool EsUnico { get; set; }

    public int Stock { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [StringLength(100)]
    public string Categoria { get; set; } = null!;

    [StringLength(120)]
    public string DropNombre { get; set; } = null!;

    public DateOnly? FechaPublicacion { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? PromedioCalificacion { get; set; }

    public int CantidadResenas { get; set; }
}
