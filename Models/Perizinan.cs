// Models/Perizinan.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public enum StatusIzin { Menunggu, Disetujui, Ditolak }

    public class Perizinan
    {
        [Key]
        public int IdIzin { get; set; }

        [Required]
        public int IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public User? User { get; set; }

        [Required]
        public DateTime Tanggal { get; set; }

        [Required, MaxLength(500)]
        public string Alasan { get; set; } = string.Empty;

        [Required]
        public string FileBukti { get; set; } = string.Empty;

        [Required]
        public StatusIzin Status { get; set; } = StatusIzin.Menunggu;

        public int? DiprosesOlehIdUser { get; set; }
        [ForeignKey(nameof(DiprosesOlehIdUser))]
        public User? DiprosesOleh { get; set; }
    }
}