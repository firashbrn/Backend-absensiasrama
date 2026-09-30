// Models/Dto/PerizinanDto.cs
namespace App.Models.Dto
{
    public class PerizinanResponseDto
    {
        public int IdIzin { get; set; }
        public string NamaPenghuni { get; set; } = string.Empty;
        public DateTime Tanggal { get; set; }
        public string Alasan { get; set; } = string.Empty;
        public string FileBukti { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}