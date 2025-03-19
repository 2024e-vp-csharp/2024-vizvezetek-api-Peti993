using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vizvezetek.API.DTos;
using Vizvezetek.API.Models;
using Vizvezetek.API.Helpers;

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

        // POST api/Munkalapok
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] munkalap munkalap)
        {
            _context.munkalap.Add(munkalap);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = munkalap.id }, munkalap);
        }

        // PUT api/Munkalapok/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] munkalap munkalap)
        {
            if (id != munkalap.id)
                return BadRequest();

            _context.Entry(munkalap).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/Munkalapok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var munkalap = await _context.munkalap.FindAsync(id);
            if (munkalap == null)
                return NotFound();

            _context.munkalap.Remove(munkalap);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
