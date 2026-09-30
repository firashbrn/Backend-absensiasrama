// Controllers/GedungController.cs
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
    public class GedungController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GedungController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GedungResponseDto>>> GetAll()
        {
            var data = await _context.Gedungs
                .Include(g => g.PenanggungJawab)
                .Select(g => new GedungResponseDto
                {
                    IdGedung = g.IdGedung,
                    NamaGedung = g.NamaGedung,
                    NamaPenanggungJawab = g.PenanggungJawab != null ? g.PenanggungJawab.Nama : null
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(GedungCreateDto dto)
        {
            var gedung = new Gedung { NamaGedung = dto.NamaGedung, IdUser = dto.IdUser };
            _context.Gedungs.Add(gedung);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Gedung ditambahkan", idGedung = gedung.IdGedung });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, GedungCreateDto dto)
        {
            var gedung = await _context.Gedungs.FindAsync(id);
            if (gedung == null) return NotFound();

            gedung.NamaGedung = dto.NamaGedung;
            gedung.IdUser = dto.IdUser;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Gedung diperbarui" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var gedung = await _context.Gedungs.FindAsync(id);
            if (gedung == null) return NotFound();

            var punyaKamar = await _context.Kamars.AnyAsync(k => k.IdGedung == id);
            if (punyaKamar) return BadRequest(new { message = "Hapus dulu semua kamar di gedung ini" });

            _context.Gedungs.Remove(gedung);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Gedung dihapus" });
        }
    }
}