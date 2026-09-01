using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

public partial class Drop
{
    [Key]
    public int IdDrop { get; set; }

    [StringLength(120)]
    public string Nombre { get; set; } = null!;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    public DateOnly? FechaPublicacion { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdDropNavigation")]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
