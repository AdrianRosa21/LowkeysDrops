using System;
using System.Collections.Generic;
using LowkeysDrops.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LowkeysDrops.API.Data;

public partial class LowkeysDropsDbContext : DbContext
{
    public LowkeysDropsDbContext(DbContextOptions<LowkeysDropsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Drop> Drops { get; set; }

    public virtual DbSet<Entrega> Entregas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Resena> Resenas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VwAuditoriaReciente> VwAuditoriaRecientes { get; set; }

    public virtual DbSet<VwCatalogoDisponible> VwCatalogoDisponibles { get; set; }

    public virtual DbSet<VwPedidosDisponiblesRepartidor> VwPedidosDisponiblesRepartidors { get; set; }

    public virtual DbSet<VwPedidosResuman> VwPedidosResumen { get; set; }

    public virtual DbSet<VwVentasEntregada> VwVentasEntregadas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA09363B218");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UsuarioBd).HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A10241BF42D");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__DetalleP__E43646A5A2AD2CE5");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalle_Pedido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalle_Producto");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion).HasName("PK__Direccio__1F8E0C76051F639E");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Direccions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Direccion_Usuario");
        });

        modelBuilder.Entity<Drop>(entity =>
        {
            entity.HasKey(e => e.IdDrop).HasName("PK__Drops__F6DEC6A7FD5872F5");

            entity.Property(e => e.Estado).HasDefaultValue("BORRADOR");
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.IdEntrega).HasName("PK__Entrega__C852F553E10653A5");

            entity.ToTable("Entrega", tb =>
                {
                    tb.HasTrigger("TR_AUD_Entrega");
                    tb.HasTrigger("TR_Entrega_ValidarRepartidor");
                });

            entity.Property(e => e.Estado).HasDefaultValue("PENDIENTE");
            entity.Property(e => e.FechaTomado).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdPedidoNavigation).WithOne(p => p.Entrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entrega_Pedido");

            entity.HasOne(d => d.IdRepartidorNavigation).WithMany(p => p.Entregas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entrega_Repartidor");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__FC851A3A6C61986F");

            entity.ToTable("Pago", tb => tb.HasTrigger("TR_AUD_Pago"));

            entity.Property(e => e.Estado).HasDefaultValue("PENDIENTE");

            entity.HasOne(d => d.IdPedidoNavigation).WithOne(p => p.Pago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Pedido");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC3C7832735");

            entity.ToTable("Pedido", tb =>
                {
                    tb.HasTrigger("TR_AUD_Pedido");
                    tb.HasTrigger("TR_Pedido_ValidarClienteDireccion");
                });

            entity.Property(e => e.Estado).HasDefaultValue("PENDIENTE");
            entity.Property(e => e.FechaPedido).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Cliente");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Direccion");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__0988921037C5ADB6");

            entity.ToTable("Producto", tb => tb.HasTrigger("TR_AUD_Producto"));

            entity.Property(e => e.EsUnico).HasDefaultValue(true);
            entity.Property(e => e.Estado).HasDefaultValue("DISPONIBLE");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Stock).HasDefaultValue(1);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.IdDropNavigation).WithMany(p => p.Productos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Drop");
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.IdResena).HasName("PK__Resena__A53BB7F87EF93197");

            entity.ToTable("Resena", tb =>
                {
                    tb.HasTrigger("TR_AUD_Resena");
                    tb.HasTrigger("TR_Resena_ValidarCompraEntregada");
                });

            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Resenas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resena_Cliente");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Resenas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resena_Producto");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF9705E8A974");

            entity.ToTable("Usuario", tb => tb.HasTrigger("TR_AUD_Usuario"));

            entity.HasIndex(e => e.Dui, "UX_Usuario_DUI")
                .IsUnique()
                .HasFilter("([DUI] IS NOT NULL)");

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Rol).HasDefaultValue("CLIENTE");
        });

        modelBuilder.Entity<VwAuditoriaReciente>(entity =>
        {
            entity.ToView("vw_AuditoriaReciente");

            entity.Property(e => e.IdAuditoria).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<VwCatalogoDisponible>(entity =>
        {
            entity.ToView("vw_CatalogoDisponible");
        });

        modelBuilder.Entity<VwPedidosDisponiblesRepartidor>(entity =>
        {
            entity.ToView("vw_PedidosDisponiblesRepartidor");
        });

        modelBuilder.Entity<VwPedidosResuman>(entity =>
        {
            entity.ToView("vw_PedidosResumen");
        });

        modelBuilder.Entity<VwVentasEntregada>(entity =>
        {
            entity.ToView("vw_VentasEntregadas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
