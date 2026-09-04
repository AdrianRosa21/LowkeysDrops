using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Table("Usuario")]
[Index("Correo", Name = "UQ_Usuario_Correo", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(150)]
    public string Correo { get; set; } = null!;

    [StringLength(255)]
    public string ContrasenaHash { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [Column("DUI")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Dui { get; set; }

    [StringLength(500)]
    public string? FotoPerfilUrl { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Rol { get; set; } = null!;

    public bool RequierePagoAnticipado { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();

    [InverseProperty("IdRepartidorNavigation")]
    public virtual ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
}
