using System.Configuration;
using Microsoft.EntityFrameworkCore;

// Código generado automáticamente por EF Tools junto a las clases a las que hace referencia
// Permite ingeniería directa e inversa => SQL to Model && Model to SQL
// Scaffold-DbContext "server=localhost;port=3306;user=root;password=yourpassword;database=test_scaffolding" MySql.Data.EntityFrameworkCore -OutputDir DataAccess\DataObjects -f

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
                optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
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

                entity.HasIndex(e => e.RepartoIdReparto)
                    .HasName("fk_RepartoRole_idReparto");

                entity.HasIndex(e => e.RoleIdRole)
                    .HasName("fk_RepartoRole_idRole");

                entity.Property(e => e.RepartoIdReparto).HasColumnName("Reparto_idReparto");

                entity.Property(e => e.RoleIdRole).HasColumnName("Role_idRole");

                entity.HasOne(d => d.RepartoIdRepartoNavigation)
                    .WithMany(p => p.RepartoRole)
                    .HasForeignKey(d => d.RepartoIdReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RepartoRole_idReparto");

                entity.HasOne(d => d.RoleIdRoleNavigation)
                    .WithMany(p => p.RepartoRole)
                    .HasForeignKey(d => d.RoleIdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RepartoRole_idRole");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.IdReview)
                    .HasName("PRIMARY");

                entity.ToTable("review");

                entity.HasIndex(e => e.SerieIdSerie)
                    .HasName("fk_Review_idSerie");

                entity.HasIndex(e => e.UsuarioIdUsuario)
                    .HasName("fk_Review_idUsuario");

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
                    .HasConstraintName("fk_Review_idSerie");

                entity.HasOne(d => d.UsuarioIdUsuarioNavigation)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.UsuarioIdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Review_idUsuario");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PRIMARY");

                entity.ToTable("role");

                entity.HasIndex(e => e.Nombre)
                .HasName("nombre_UNIQUE")
                .IsUnique();

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("enum('director','writer','actor')");
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
                    .HasMaxLength(1000);

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
                    .HasName("fk_SerieGenero_idGenero");

                entity.HasIndex(e => e.SerieIdSerie)
                    .HasName("fk_SerieGenero_idSerie");

                entity.Property(e => e.GeneroIdGenero).HasColumnName("Genero_idGenero");

                entity.Property(e => e.SerieIdSerie).HasColumnName("Serie_idSerie");

                entity.HasOne(d => d.GeneroIdGeneroNavigation)
                    .WithMany(p => p.SerieGenero)
                    .HasForeignKey(d => d.GeneroIdGenero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerieGenero_idGenero");

                entity.HasOne(d => d.SerieIdSerieNavigation)
                    .WithMany(p => p.SerieGenero)
                    .HasForeignKey(d => d.SerieIdSerie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerieGenero_idSerie");
            });

            modelBuilder.Entity<SerieReparto>(entity =>
            {
                entity.HasKey(e => new { e.SerieIdSerie, e.RepartoIdReparto })
                    .HasName("PRIMARY");

                entity.ToTable("serie_reparto");

                entity.HasIndex(e => e.RepartoIdReparto)
                    .HasName("fk_SerieReparto_idReparto");

                entity.HasIndex(e => e.SerieIdSerie)
                    .HasName("fk_SerieReparto_idSerie");

                entity.Property(e => e.RepartoIdReparto).HasColumnName("Reparto_idReparto");

                entity.Property(e => e.SerieIdSerie).HasColumnName("Serie_idSerie");

                entity.HasOne(d => d.RepartoIdRepartoNavigation)
                    .WithMany(p => p.SerieReparto)
                    .HasForeignKey(d => d.RepartoIdReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerieReparto_idReparto");

                entity.HasOne(d => d.SerieIdSerieNavigation)
                    .WithMany(p => p.SerieReparto)
                    .HasForeignKey(d => d.SerieIdSerie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SerieReparto_idSerie");
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

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(250);

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
