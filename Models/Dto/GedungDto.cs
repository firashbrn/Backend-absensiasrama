// Models/Dto/GedungDto.cs
namespace App.Models.Dto
{
    public class GedungCreateDto
    {
        public string NamaGedung { get; set; } = string.Empty;
        public int? IdUser { get; set; }
    }

    public class GedungResponseDto
    {
        public int IdGedung { get; set; }
        public string NamaGedung { get; set; } = string.Empty;
        public string? NamaPenanggungJawab { get; set; }
    }
}