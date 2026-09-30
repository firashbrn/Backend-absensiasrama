// Models/Presensi.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public enum StatusPresensi { Hadir, Izin, Alpa }
    public enum SesiPresensi { Subuh, Malam }

    public class Presensi
    {
        [Key]
        public int IdPresensi { get; set; }

        [Required]
        public DateTime Waktu { get; set; }

        [Required]
        public DateTime Tanggal { get; set; }

        [Required]
        public SesiPresensi Sesi { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public double Latitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public double Longitude { get; set; }

        [Required]
        public StatusPresensi Status { get; set; }

        public string? FotoWajah { get; set; }
        public string? Keterangan { get; set; }

        [Required]
        public int IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public User? User { get; set; }
    }
}