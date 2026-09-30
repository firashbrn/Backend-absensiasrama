// Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public enum Role { Admin, Fasil, Penghuni }

    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required, MaxLength(100)]
        public string Nama { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Nim { get; set; }

        [MaxLength(100)]
        public string? Asal { get; set; }

        public string? JenisKelamin { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        public int? NoKamar { get; set; }
        [ForeignKey(nameof(NoKamar))]
        public Kamar? Kamar { get; set; }

        public ICollection<Presensi>? DaftarPresensi { get; set; }
        public ICollection<Perizinan>? DaftarPerizinan { get; set; }
        public ICollection<Gedung>? GedungDikelola { get; set; }
    }
}