// Models/Kamar.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Kamar
    {
        [Key]
        public int NoKamar { get; set; }

        [Required, MaxLength(10)]
        public string Lantai { get; set; } = string.Empty;

        [Required]
        public int IdGedung { get; set; }
        [ForeignKey(nameof(IdGedung))]
        public Gedung? Gedung { get; set; }

        public ICollection<User>? Penghuni { get; set; }
    }
}