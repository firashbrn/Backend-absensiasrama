// Models/Dto/UserDto.cs
namespace App.Models.Dto
{
    public class UserDto
    {
        public int IdUser { get; set; }
        public string Nama { get; set; } = string.Empty;
        public string? Nim { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? NoKamar { get; set; }
    }
}