// Models/Dto/KamarDto.cs
namespace App.Models.Dto
{
    public class KamarCreateDto
    {
        public string Lantai { get; set; } = string.Empty;
        public int IdGedung { get; set; }
    }

    public class KamarResponseDto
    {
        public int NoKamar { get; set; }
        public string Lantai { get; set; } = string.Empty;
        public string NamaGedung { get; set; } = string.Empty;
        public int JumlahPenghuni { get; set; }
    }
}