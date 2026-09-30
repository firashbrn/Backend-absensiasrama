// Controllers/PerizinanController.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Dto;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PerizinanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private static readonly string[] ExtensiDiizinkan = { ".jpg", ".jpeg", ".pdf" };
        private const long MaksUkuranByte = 5 * 1024 * 1024;

        public PerizinanController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost]
        [RequestSizeLimit(MaksUkuranByte)]
        public async Task<ActionResult> AjukanIzin(
            [FromForm] DateTime tanggal,
            [FromForm] string alasan,
            [FromForm] IFormFile fileBukti)
        {
            var extensi = Path.GetExtension(fileBukti.FileName).ToLower();
            if (!ExtensiDiizinkan.Contains(extensi))
                return BadRequest(new { message = "File harus .jpg atau .pdf" });

            if (fileBukti.Length > MaksUkuranByte)
                return BadRequest(new { message = "Ukuran file maksimal 5MB" });

            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var folderUpload = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "izin");
            Directory.CreateDirectory(folderUpload);

            var namaFileUnik = $"{Guid.NewGuid()}{extensi}";
            var pathFisik = Path.Combine(folderUpload, namaFileUnik);

            using (var stream = new FileStream(pathFisik, FileMode.Create))
            {
                await fileBukti.CopyToAsync(stream);
            }

            var perizinan = new Perizinan
            {
                IdUser = idUser,
                Tanggal = tanggal,
                Alasan = alasan,
                FileBukti = $"/uploads/izin/{namaFileUnik}",
                Status = StatusIzin.Menunggu
            };

            _context.Perizinans.Add(perizinan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pengajuan izin terkirim", idIzin = perizinan.IdIzin });
        }

        [HttpGet]
        [Authorize(Roles = "Fasil,Admin")]
        public async Task<ActionResult<IEnumerable<PerizinanResponseDto>>> DaftarPengajuan()
        {
            var daftar = await _context.Perizinans
                .Include(p => p.User)
                .OrderByDescending(p => p.Tanggal)
                .Select(p => new PerizinanResponseDto
                {
                    IdIzin = p.IdIzin,
                    NamaPenghuni = p.User!.Nama,
                    Tanggal = p.Tanggal,
                    Alasan = p.Alasan,
                    FileBukti = p.FileBukti,
                    Status = p.Status.ToString()
                })
                .ToListAsync();

            return Ok(daftar);
        }

        [HttpPut("{id}/proses")]
        [Authorize(Roles = "Fasil,Admin")]
        public async Task<ActionResult> ProsesIzin(int id, [FromBody] bool disetujui)
        {
            var izin = await _context.Perizinans.FindAsync(id);
            if (izin == null) return NotFound();

            var idFasil = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            izin.Status = disetujui ? StatusIzin.Disetujui : StatusIzin.Ditolak;
            izin.DiprosesOlehIdUser = idFasil;

            if (disetujui)
            {
                var presensi = await _context.Presensis.FirstOrDefaultAsync(p =>
                    p.IdUser == izin.IdUser && p.Tanggal.Date == izin.Tanggal.Date);

                if (presensi != null)
                {
                    presensi.Status = StatusPresensi.Izin;
                    presensi.Keterangan = izin.Alasan;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Pengajuan diproses" });
        }
    }
}