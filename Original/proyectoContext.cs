using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class proyectoContext : DbContext
    {
        public proyectoContext()
        {
        }

        public proyectoContext(DbContextOptions<proyectoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Reparto> Reparto { get; set; }
        public virtual DbSet<RepartoRole> RepartoRole { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Serie> Serie { get; set; }
        public virtual DbSet<SerieGenero> SerieGenero { get; set; }
        public virtual DbSet<SerieReparto> SerieReparto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;database=proyecto;userid=carlos;pwd=carlos");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero)
                    .HasName("PRIMARY");

                entity.ToTable("genero");

                entity.Property(e => e.IdGenero).HasColumnName("idGenero");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Reparto>(entity =>
            {
                entity.HasKey(e => e.IdReparto)
                    .HasName("PRIMARY");

                entity.ToTable("reparto");

                entity.Property(e => e.IdReparto).HasColumnName("idReparto");

                entity.Property(e => e.Foto)
                    .HasColumnName("foto")
                    .HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<RepartoRole>(entity =>
            {
                entity.HasKey(e => new { e.RepartoIdReparto, e.RoleIdRole })
                    .HasName("PRIMARY");

                entity.ToTable("reparto_role");

                entity.HasIndex(e => e.RoleIdRole)
                    .HasName("fk_RepRol_Role");

                entity.Property(e => e.RepartoIdReparto).HasColumnName("Reparto_idReparto");

                entity.Property(e => e.RoleIdRole).HasColumnName("Role_idRole");

                entity.HasOne(d => d.RepartoIdRepartoNavigation)
                    .WithMany(p => p.RepartoRole)
                    .HasForeignKey(d => d.RepartoIdReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RepRol_Reparto");

                entity.HasOne(d => d.RoleIdRoleNavigation)
                    .WithMany(p => p.RepartoRole)
                    .HasForeignKey(d => d.RoleIdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RepRol_Role");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.IdReview)
                    .HasName("PRIMARY");

                entity.ToTable("review");

                entity.HasIndex(e => e.SerieIdSerie)
                    .HasName("fk_Review_Serie");

                entity.HasIndex(e => e.UsuarioIdUsuario)
                    .HasName("fk_Review_Usuario");

                entity.Property(e => e.IdReview).HasColumnName("idReview");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .HasMaxLength(45);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Puntuacion).HasColumnName("puntuacion");

                entity.Property(e => e.SerieIdSerie).HasColumnName("Serie_idSerie");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(45);

                entity.Property(e => e.UsuarioIdUsuario).HasColumnName("Usuario_idUsuario");

                entity.HasOne(d => d.SerieIdSerieNavigation)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.SerieIdSerie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Review_Serie");

                entity.HasOne(d => d.UsuarioIdUsuarioNavigation)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.UsuarioIdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Review_Usuario");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PRIMARY");

                entity.ToTable("role");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Serie>(entity =>
            {
                entity.HasKey(e => e.IdSerie)
                    .HasName("PRIMARY");

                entity.ToTable("serie");

                entity.Property(e => e.IdSerie).HasColumnName("idSerie");

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasColumnName("poster")
                    .HasMaxLength(250);

                entity.Property(e => e.Sinopsis)
                    .IsRequired()
                    .HasColumnName("sinopsis")
                    .HasMaxLength(350);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(45);

                entity.Property(e => e.Trailer)
                    .IsRequired()
                    .HasColumnName("trailer")
                    .HasMaxLength(250);

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<SerieGenero>(entity =>
            {
                entity.HasKey(e => new { e.SerieIdSerie, e.GeneroIdGenero })
                    .HasName("PRIMARY");

                entity.ToTable("serie_genero");

                entity.HasIndex(e => e.GeneroIdGenero)
                    .HasName("fk_SerGen_Genero");

                entity.Property(e => e.SerieIdSerie).HasColumnName("Serie_idSerie");

                entity.Property(e => e.GeneroIdGenero).HasColumnName("Genero_idGenero");

                entity.HasOne(d => d.GeneroIdGeneroNavigation)
                    .WithMany(p => p.SerieGenero)
                    .HasForeignKey(d => d.GeneroIdGenero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerGen_Genero");

                entity.HasOne(d => d.SerieIdSerieNavigation)
                    .WithMany(p => p.SerieGenero)
                    .HasForeignKey(d => d.SerieIdSerie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerGen_Serie");
            });

            modelBuilder.Entity<SerieReparto>(entity =>
            {
                entity.HasKey(e => new { e.SerieIdSerie, e.RepartoIdReparto })
                    .HasName("PRIMARY");

                entity.ToTable("serie_reparto");

                entity.HasIndex(e => e.RepartoIdReparto)
                    .HasName("fk_SerRep_Reparto");

                entity.Property(e => e.SerieIdSerie).HasColumnName("Serie_idSerie");

                entity.Property(e => e.RepartoIdReparto).HasColumnName("Reparto_idReparto");

                entity.HasOne(d => d.RepartoIdRepartoNavigation)
                    .WithMany(p => p.SerieReparto)
                    .HasForeignKey(d => d.RepartoIdReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerRep_Reparto");

                entity.HasOne(d => d.SerieIdSerieNavigation)
                    .WithMany(p => p.SerieReparto)
                    .HasForeignKey(d => d.SerieIdSerie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerRep_Serie");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("username_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(45);

                entity.Property(e => e.FotoPerfil)
                    .HasColumnName("foto_perfil")
                    .HasMaxLength(250);

                entity.Property(e => e.Genero)
                    .HasColumnName("genero")
                    .HasColumnType("enum('masculino','femenino','no binario')");

                entity.Property(e => e.Nacimiento)
                    .HasColumnName("nacimiento")
                    .HasColumnType("date");

                entity.Property(e => e.Pais)
                    .HasColumnName("pais")
                    .HasMaxLength(45);

                entity.Property(e => e.PasswordSha512)
                    .IsRequired()
                    .HasColumnName("password_sha512")
                    .HasColumnType("binary(128)");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasColumnType("enum('admin','normal')");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
