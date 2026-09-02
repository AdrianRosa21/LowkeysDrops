using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Resena")]
[Index("IdProducto", Name = "IX_Resena_Producto")]
[Index("IdProducto", "IdCliente", Name = "UQ_Resena_ProductoCliente", IsUnique = true)]
public partial class Resena
{
    [Key]
    public int IdResena { get; set; }

    public int IdProducto { get; set; }

    public int IdCliente { get; set; }

    public byte Calificacion { get; set; }

    [StringLength(800)]
    public string? Comentario { get; set; }

    public DateTime Fecha { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("Resenas")]
    public virtual Usuario IdClienteNavigation { get; set; } = null!;

    [ForeignKey("IdProducto")]
    [InverseProperty("Resenas")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
