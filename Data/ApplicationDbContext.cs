// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Gedung> Gedungs { get; set; }
    public DbSet<Kamar> Kamars { get; set; }
    public DbSet<Presensi> Presensis { get; set; }
    public DbSet<Perizinan> Perizinans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Nim).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>()
    .HasCheckConstraint("CK_User_JenisKelamin", "JenisKelamin IN ('L', 'P')");

        modelBuilder.Entity<User>()
            .HasOne(u => u.Kamar)
            .WithMany(k => k.Penghuni)
            .HasForeignKey(u => u.NoKamar)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Gedung>()
            .HasOne(g => g.PenanggungJawab)
            .WithMany(u => u.GedungDikelola)
            .HasForeignKey(g => g.IdUser)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Kamar>()
            .HasOne(k => k.Gedung)
            .WithMany(g => g.DaftarKamar)
            .HasForeignKey(k => k.IdGedung)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Kamar>().HasIndex(k => k.IdGedung);

        modelBuilder.Entity<Presensi>()
            .HasOne(p => p.User)
            .WithMany(u => u.DaftarPresensi)
            .HasForeignKey(p => p.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Presensi>()
            .HasIndex(p => new { p.IdUser, p.Tanggal, p.Sesi })
            .IsUnique();
        modelBuilder.Entity<Presensi>().HasIndex(p => p.Tanggal);

        modelBuilder.Entity<Perizinan>()
            .HasOne(p => p.User)
            .WithMany(u => u.DaftarPerizinan)
            .HasForeignKey(p => p.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Perizinan>()
            .HasOne(p => p.DiprosesOleh)
            .WithMany()
            .HasForeignKey(p => p.DiprosesOlehIdUser)
            .OnDelete(DeleteBehavior.Restrict);
    }
}