// Models/Dto/AuthDto.cs
namespace App.Models.Dto
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class RegisterRequestDto
    {
        public string Nama { get; set; } = string.Empty;
        public string? Nim { get; set; }
        public string? Asal { get; set; }
        public string? JenisKelamin { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? NoKamar { get; set; }
    }
}