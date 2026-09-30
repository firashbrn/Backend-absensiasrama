// Models/Gedung.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Gedung
    {
        [Key]
        public int IdGedung { get; set; }

        [Required, MaxLength(100)]
        public string NamaGedung { get; set; } = string.Empty;

        public int? IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public User? PenanggungJawab { get; set; }

        public ICollection<Kamar>? DaftarKamar { get; set; }
    }
}