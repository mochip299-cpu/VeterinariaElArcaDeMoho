using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Models;

namespace Veterinaria.Data;

public class VeterinariaDbContext : IdentityDbContext<IdentityUser>
{
    public VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> options)
        : base(options) { }

    public DbSet<Propietario> Propietarios => Set<Propietario>();
    public DbSet<Mascota>     Mascotas     => Set<Mascota>();
    public DbSet<Veterinario> Veterinarios => Set<Veterinario>();
    public DbSet<Cita>        Citas        => Set<Cita>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Propietario ──────────────────────────────────────────────────────
        modelBuilder.Entity<Propietario>(e =>
        {
            e.ToTable("Propietarios");
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            e.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
            e.Property(p => p.Telefono).IsRequired().HasMaxLength(20);
            e.Property(p => p.Correo).IsRequired().HasMaxLength(150);
            e.Property(p => p.Estado).IsRequired().HasConversion<int>();
            e.HasIndex(p => p.Correo).IsUnique();
        });

        // ── Mascota ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Mascota>(e =>
        {
            e.ToTable("Mascotas");
            e.HasKey(m => m.Id);
            e.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
            e.Property(m => m.Especie).IsRequired().HasMaxLength(60);
            e.Property(m => m.Raza).HasMaxLength(80);
            e.Property(m => m.Color).IsRequired().HasMaxLength(60);
            e.Property(m => m.Estado).IsRequired().HasConversion<int>();
            e.HasOne(m => m.Propietario)
                .WithMany(p => p.Mascotas)
                .HasForeignKey(m => m.PropietarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Veterinario ──────────────────────────────────────────────────────
        modelBuilder.Entity<Veterinario>(e =>
        {
            e.ToTable("Veterinarios");
            e.HasKey(v => v.Id);
            e.Property(v => v.Nombre).IsRequired().HasMaxLength(100);
            e.Property(v => v.Apellido).IsRequired().HasMaxLength(100);
            e.Property(v => v.Especialidad).IsRequired().HasMaxLength(120);
            e.Property(v => v.Telefono).IsRequired().HasMaxLength(20);
            e.Property(v => v.Estado).IsRequired().HasConversion<int>();
        });

        // ── Cita ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Cita>(e =>
        {
            e.ToTable("Citas");
            e.HasKey(c => c.Id);
            e.Property(c => c.Motivo).IsRequired().HasMaxLength(300);
            e.Property(c => c.Diagnostico).HasMaxLength(1000);
            e.Property(c => c.Estado).IsRequired().HasConversion<int>();
            e.Property(c => c.CreadoEn).IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.HasOne(c => c.Mascota)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.MascotaId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Veterinario)
                .WithMany(v => v.Citas)
                .HasForeignKey(c => c.VeterinarioId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(c => c.FechaHoraAtencion);
        });
    }
}
