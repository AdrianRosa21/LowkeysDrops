using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Direccion")]
[Index("IdUsuario", Name = "IX_Direccion_IdUsuario")]
[Index("IdUsuario", "Tipo", Name = "UQ_Direccion_UsuarioTipo", IsUnique = true)]
public partial class Direccion
{
    [Key]
    public int IdDireccion { get; set; }

    public int IdUsuario { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Tipo { get; set; } = null!;

    [StringLength(80)]
    public string Departamento { get; set; } = null!;

    [StringLength(100)]
    public string Municipio { get; set; } = null!;

    [StringLength(300)]
    public string DireccionTexto { get; set; } = null!;

    [StringLength(300)]
    public string? Referencia { get; set; }

    public DateTime FechaRegistro { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Direccions")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdDireccionNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
