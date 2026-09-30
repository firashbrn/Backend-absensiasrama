// Models/Dto/PresensiResponseDto.cs
namespace App.Models.Dto
{
    public class PresensiResponseDto
    {
        public int IdPresensi { get; set; }
        public DateTime Waktu { get; set; }
        public string Sesi { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Keterangan { get; set; }
    }
}