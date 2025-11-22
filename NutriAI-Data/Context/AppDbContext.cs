using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NutriAI_Data.Models;

namespace NutriAI_Data.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comuna> Comunas { get; set; }

    public virtual DbSet<Conversacione> Conversaciones { get; set; }

    public virtual DbSet<Enfermedade> Enfermedades { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<MensajesConversacion> MensajesConversacions { get; set; }

    public virtual DbSet<NivelesActividad> NivelesActividads { get; set; }

    public virtual DbSet<PdfDocument> PdfDocuments { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<RecetasPdf> RecetasPdfs { get; set; }

    public virtual DbSet<RecetasPdfDetalle> RecetasPdfDetalles { get; set; }

    public virtual DbSet<Sexo> Sexos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioEnfermedad> UsuarioEnfermedads { get; set; }

    public virtual DbSet<UsuarioIngrediente> UsuarioIngredientes { get; set; }

    public virtual DbSet<UsuarioPerfil> UsuarioPerfils { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-5AJE7OS;Database=NutriAI;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comuna>(entity =>
        {
            entity.HasKey(e => e.IdComuna).HasName("PK__Comunas__09BC277975F4663A");

            entity.ToTable("Comunas", "Entidad");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<Conversacione>(entity =>
        {
            entity.HasKey(e => e.IdConversacion);

            entity.HasIndex(e => e.FechaCreacion, "IX_Conversaciones_FechaCreacion").IsDescending();

            entity.HasIndex(e => e.IdUsuario, "IX_Conversaciones_IdUsuario");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Conversaciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Conversaciones_Usuarios");
        });

        modelBuilder.Entity<Enfermedade>(entity =>
        {
            entity.HasKey(e => e.IdEnfermedad).HasName("PK__Enfermed__41C48E55512D5B5A");

            entity.ToTable("Enfermedades", "Maestro");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente).HasName("PK__Ingredie__3DA4DD60939D63AE");

            entity.ToTable("Ingredientes", "Maestro");

            entity.Property(e => e.Calorias).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Carbohidratos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Categoria).HasMaxLength(100);
            entity.Property(e => e.Grasas).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Proteinas).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<MensajesConversacion>(entity =>
        {
            entity.HasKey(e => e.IdMensaje);

            entity.ToTable("MensajesConversacion");

            entity.HasIndex(e => e.FechaEnvio, "IX_MensajesConversacion_FechaEnvio").IsDescending();

            entity.HasIndex(e => e.IdConversacion, "IX_MensajesConversacion_IdConversacion");

            entity.Property(e => e.FechaEnvio).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdConversacionNavigation).WithMany(p => p.MensajesConversacions)
                .HasForeignKey(d => d.IdConversacion)
                .HasConstraintName("FK_MensajesConversacion_Conversaciones");
        });

        modelBuilder.Entity<NivelesActividad>(entity =>
        {
            entity.HasKey(e => e.IdNivelActividad).HasName("PK__NivelesA__8AEE982A1DFC92B8");

            entity.ToTable("NivelesActividad", "Entidad");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(150);
        });

        modelBuilder.Entity<PdfDocument>(entity =>
        {
            entity.HasKey(e => e.IdPdfDocument).HasName("PK__PdfDocum__0EE008FC7369E807");

            entity.HasIndex(e => e.FechaCreacion, "IX_PdfDocuments_FechaCreacion");

            entity.HasIndex(e => e.IdConversation, "IX_PdfDocuments_IdConversation");

            entity.HasIndex(e => e.IdUsuario, "IX_PdfDocuments_IdUsuario");

            entity.Property(e => e.ContentType)
                .HasMaxLength(50)
                .HasDefaultValue("application/pdf");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.IdConversationNavigation).WithMany(p => p.PdfDocuments)
                .HasForeignKey(d => d.IdConversation)
                .HasConstraintName("FK__PdfDocume__IdCon__5224328E");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.PdfDocuments)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PdfDocume__IdUsu__51300E55");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PK__Perfil__C7BD5CC1B5894EBE");

            entity.ToTable("Perfil", "Maestro");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(e => e.IdReceta);

            entity.HasIndex(e => e.FechaCreacion, "IX_Recetas_FechaCreacion").IsDescending();

            entity.HasIndex(e => e.IdUsuario, "IX_Recetas_IdUsuario");

            entity.HasIndex(e => e.Nombre, "IX_Recetas_Nombre");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CaloriasTotales).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CarbohidratosTotales).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.GrasasTotales).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Porciones).HasDefaultValue(1);
            entity.Property(e => e.ProteinasTotales).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Recetas_Usuarios");
        });

        modelBuilder.Entity<RecetasPdf>(entity =>
        {
            entity.HasKey(e => e.IdRecetaPdf);

            entity.ToTable("RecetasPDF");

            entity.HasIndex(e => e.FechaGeneracion, "IX_RecetasPDF_FechaGeneracion").IsDescending();

            entity.HasIndex(e => e.IdUsuario, "IX_RecetasPDF_IdUsuario");

            entity.Property(e => e.IdRecetaPdf).HasColumnName("IdRecetaPDF");
            entity.Property(e => e.ArchivoPdf).HasColumnName("ArchivoPDF");
            entity.Property(e => e.FechaGeneracion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.NombrePdf)
                .HasMaxLength(200)
                .HasColumnName("NombrePDF");
            entity.Property(e => e.NumeroRecetas).HasDefaultValue(1);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RecetasPdfs)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_RecetasPDF_Usuarios");
        });

        modelBuilder.Entity<RecetasPdfDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RecetasPDF_Detalle");

            entity.Property(e => e.IdDetalle).ValueGeneratedOnAdd();
            entity.Property(e => e.IdRecetaPdf).HasColumnName("IdRecetaPDF");
            entity.Property(e => e.Orden).HasDefaultValue(1);
        });

        modelBuilder.Entity<Sexo>(entity =>
        {
            entity.HasKey(e => e.IdSexo).HasName("PK__Sexos__A7739FA2139BBADB");

            entity.ToTable("Sexos", "Entidad");

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97B53436D9");

            entity.ToTable("Usuarios", "Entidad");

            entity.HasIndex(e => e.Username, "UQ__Usuarios__536C85E47513AA9F").IsUnique();

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19D732BB83").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Altura).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.PasswordSalt).HasMaxLength(256);
            entity.Property(e => e.Peso).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.IdComunaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdComuna)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Comuna");

            entity.HasOne(d => d.IdNivelActividadNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdNivelActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_NivelActividad");

            entity.HasOne(d => d.IdSexoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSexo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Sexo");
        });

        modelBuilder.Entity<UsuarioEnfermedad>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioEnfermedad).HasName("PK__UsuarioE__5DAD9EFF5498BEDE");

            entity.ToTable("UsuarioEnfermedad", "Entidad");

            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEnfermedadNavigation).WithMany(p => p.UsuarioEnfermedads)
                .HasForeignKey(d => d.IdEnfermedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioEnfermedad_Enfermedad");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioEnfermedads)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioEnfermedad_Usuario");
        });

        modelBuilder.Entity<UsuarioIngrediente>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioIngrediente).HasName("PK__UsuarioI__F2987EFFA26CF529");

            entity.ToTable("UsuarioIngredientes", "Entidad");

            entity.Property(e => e.CantidadGramos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdIngredienteNavigation).WithMany(p => p.UsuarioIngredientes)
                .HasForeignKey(d => d.IdIngrediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioIngredientes_Ingredientes");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioIngredientes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioIngredientes_Usuarios");
        });

        modelBuilder.Entity<UsuarioPerfil>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdPerfil });

            entity.ToTable("UsuarioPerfil", "Maestro");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.UsuarioPerfils)
                .HasForeignKey(d => d.IdPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioPerfil_Perfil");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioPerfils)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioPerfil_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
