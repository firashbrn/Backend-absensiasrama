// Models/Dto/PresensiCreateDto.cs
namespace App.Models.Dto
{
    public class PresensiCreateDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? FotoWajah { get; set; }
    }
}