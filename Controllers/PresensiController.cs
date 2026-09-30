// Controllers/PresensiController.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Dto;
using App.Services;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PresensiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PresensiService _presensiService;

        public PresensiController(ApplicationDbContext context, PresensiService presensiService)
        {
            _context = context;
            _presensiService = presensiService;
        }

        [HttpPost]
        public async Task<ActionResult> CatatPresensi(PresensiCreateDto request)
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (waktuValid, pesanWaktu, sesi) = _presensiService.ValidasiWaktu();
            if (!waktuValid) return BadRequest(new { message = pesanWaktu });

            var (lokasiValid, pesanLokasi) = _presensiService.ValidasiLokasi(request.Latitude, request.Longitude);
            if (!lokasiValid) return BadRequest(new { message = pesanLokasi });

            var sudahAbsen = await _context.Presensis.AnyAsync(p =>
                p.IdUser == idUser && p.Tanggal.Date == DateTime.Today && p.Sesi == sesi);

            if (sudahAbsen) return BadRequest(new { message = "Sudah presensi sesi ini hari ini" });

            var presensi = new Presensi
            {
                IdUser = idUser,
                Waktu = DateTime.Now,
                Tanggal = DateTime.Today,
                Sesi = sesi!.Value,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                FotoWajah = request.FotoWajah,
                Status = StatusPresensi.Hadir
            };

            _context.Presensis.Add(presensi);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Presensi berhasil", idPresensi = presensi.IdPresensi });
        }

        [HttpGet("saya")]
        public async Task<ActionResult<IEnumerable<PresensiResponseDto>>> RiwayatSaya()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var riwayat = await _context.Presensis
                .Where(p => p.IdUser == idUser)
                .OrderByDescending(p => p.Tanggal)
                .Select(p => new PresensiResponseDto
                {
                    IdPresensi = p.IdPresensi,
                    Waktu = p.Waktu,
                    Sesi = p.Sesi.ToString(),
                    Status = p.Status.ToString(),
                    Keterangan = p.Keterangan
                })
                .ToListAsync();

            return Ok(riwayat);
        }

        [HttpGet]
        [Authorize(Roles = "Fasil,Admin")]
        public async Task<ActionResult> SemuaRiwayat()
        {
            var riwayat = await _context.Presensis
                .Include(p => p.User)
                .OrderByDescending(p => p.Tanggal)
                .Select(p => new
                {
                    p.IdPresensi,
                    p.Waktu,
                    Sesi = p.Sesi.ToString(),
                    Status = p.Status.ToString(),
                    NamaUser = p.User!.Nama
                })
                .ToListAsync();

            return Ok(riwayat);
        }
    }
}