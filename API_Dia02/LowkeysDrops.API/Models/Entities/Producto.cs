using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Producto")]
[Index("IdCategoria", Name = "IX_Producto_Categoria")]
[Index("IdDrop", Name = "IX_Producto_Drop")]
[Index("Estado", Name = "IX_Producto_Estado")]
[Index("Nombre", Name = "IX_Producto_Nombre")]
public partial class Producto
{
    [Key]
    public int IdProducto { get; set; }

    public int IdDrop { get; set; }

    public int IdCategoria { get; set; }

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

    public DateTime FechaRegistro { get; set; }

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("Productos")]
    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    [ForeignKey("IdDrop")]
    [InverseProperty("Productos")]
    public virtual Drop IdDropNavigation { get; set; } = null!;

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
}
