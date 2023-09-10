using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MiParcialito.Models
{
    public partial class rb100519Context : DbContext
    {
        public rb100519Context()
        {
        }

        public rb100519Context(DbContextOptions<rb100519Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<Docente> Docentes { get; set; }
        public virtual DbSet<Inscripcione> Inscripciones { get; set; }
        public virtual DbSet<MateriaDocente> MateriaDocentes { get; set; }
        public virtual DbSet<Materium> Materia { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DRAGON\\MSSQLSERVER01;TrustServerCertificate=True;Initial Catalog=rb100519;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.Property(e => e.Roles)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("roles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Docente>(entity =>
            {
                entity.ToTable("docente");

                entity.Property(e => e.DocenteNombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("docenteNombre");
            });

            modelBuilder.Entity<Inscripcione>(entity =>
            {
                entity.HasKey(e => new { e.EstudianteId, e.MateriaId })
                    .HasName("PK__Inscripc__DCCA85D3B6B6723E");

                entity.Property(e => e.EstudianteId).HasColumnName("EstudianteID");

                entity.Property(e => e.MateriaId).HasColumnName("materiaID");

                entity.Property(e => e.InscripcionesId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("InscripcionesID");

                entity.HasOne(d => d.Estudiante)
                    .WithMany(p => p.Inscripciones)
                    .HasForeignKey(d => d.EstudianteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inscripci__Estud__47DBAE45");

                entity.HasOne(d => d.Materia)
                    .WithMany(p => p.Inscripciones)
                    .HasForeignKey(d => d.MateriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inscripci__mater__46E78A0C");
            });

            modelBuilder.Entity<MateriaDocente>(entity =>
            {
                entity.HasKey(e => new { e.MateriaId, e.DocenteId })
                    .HasName("pk_materiadocente");

                entity.ToTable("materiaDocente");

                entity.Property(e => e.MateriaId).HasColumnName("materiaId");

                entity.Property(e => e.DocenteId).HasColumnName("docenteId");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Docente)
                    .WithMany(p => p.MateriaDocentes)
                    .HasForeignKey(d => d.DocenteId)
                    .HasConstraintName("fk_materiadocente_docenteid");

                entity.HasOne(d => d.Materia)
                    .WithMany(p => p.MateriaDocentes)
                    .HasForeignKey(d => d.MateriaId)
                    .HasConstraintName("fk_materiasdocente_materiaid");
            });

            modelBuilder.Entity<Materium>(entity =>
            {
                entity.ToTable("materia");

                entity.Property(e => e.MateriaNombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("materiaNombre");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
