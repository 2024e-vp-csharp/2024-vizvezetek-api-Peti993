using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vizvezetek.API.DTos;
using Vizvezetek.API.Models;
using Vizvezetek.API.Helpers;
using Vizvezetek.API.Dtos;

namespace Vizvezetek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunkalapokController : ControllerBase
    {
        private readonly vizvezetekContext _context;

        public MunkalapokController(vizvezetekContext context)
        {
            _context = context;
        }

        // GET api/Munkalapok
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var munkalapok = await _context.munkalap
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .ToListAsync();

            var dtos = munkalapok.Select(m => MunkalapMapper.ToDto(m));
            return Ok(dtos);
        }

        // GET api/Munkalapok/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var munkalap = await _context.munkalap
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .FirstOrDefaultAsync(m => m.id == id);

            if (munkalap == null)
                return NotFound();

            return Ok(MunkalapMapper.ToDto(munkalap));
        }

        // GET api/Munkalapok/kereses?helyId=3&szereloId=7
        [HttpGet("kereses")]
        public async Task<IActionResult> KeresesViaUrl([FromQuery] int helyId, [FromQuery] int szereloId)
        {
            var ids = await _context.munkalap
                .Where(m => m.hely_id == helyId && m.szerelo_id == szereloId)
                .Select(m => m.id)
                .ToListAsync();

            if (!ids.Any())
                return NotFound("Nincs ilyen munkalap.");

            return Ok(ids);
        }
        
        // GET api/Munkalapok/ev/2002
        [HttpGet("ev/{evszam}")]
        public async Task<IActionResult> GetLezartMunkalapokByEv(int evszam)
        {
            var munkalapok = await _context.munkalap
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .Where(m => m.javitas_datum != null &&
                            m.javitas_datum.Year == evszam)
                .ToListAsync();

            if (!munkalapok.Any())
                return NotFound($"Nincs lezárt munkalap a(z) {evszam}-es évben.");

            var dtos = munkalapok.Select(m => MunkalapMapper.ToDto(m));

            return Ok(dtos);
        }
    }
}
