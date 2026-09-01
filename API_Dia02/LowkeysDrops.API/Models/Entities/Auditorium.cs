using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Models.Entities;

[Index("Fecha", Name = "IX_Auditoria_Fecha", AllDescending = true)]
public partial class Auditorium
{
    [Key]
    public long IdAuditoria { get; set; }

    [StringLength(128)]
    public string Tabla { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Accion { get; set; } = null!;

    public long? IdRegistro { get; set; }

    public DateTime Fecha { get; set; }

    [Column("UsuarioBD")]
    [StringLength(128)]
    public string UsuarioBd { get; set; } = null!;

    public string? DatosAnteriores { get; set; }

    public string? DatosNuevos { get; set; }
}
