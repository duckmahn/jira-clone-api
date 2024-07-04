using managementapp.Data;
using managementapp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace managementapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KanbansController : ControllerBase
    {
        private readonly DataContext _context;

        public KanbansController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kanban>>> GetKanbans()
        {
          if (_context.Kanbans == null)
          {
              return NotFound();
          }
            return await _context.Kanbans.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kanban>> GetKanban(Guid id)
        {
          if (_context.Kanbans == null)
          {
              return NotFound();
          }
            var kanban = await _context.Kanbans.FindAsync(id);

            if (kanban == null)
            {
                return NotFound();
            }

            return kanban;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKanban(Guid id, Kanban kanban)
        {
            if (id != kanban.Id)
            {
                return BadRequest();
            }

            _context.Entry(kanban).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KanbanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Kanban>> PostKanban(Kanban kanban)
        {
          if (_context.Kanbans == null)
          {
              return Problem("Entity set 'DataContext.Kanbans'  is null.");
          }
            _context.Kanbans.Add(kanban);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKanban", new { id = kanban.Id }, kanban);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKanban(Guid id)
        {
            if (_context.Kanbans == null)
            {
                return NotFound();
            }
            var kanban = await _context.Kanbans.FindAsync(id);
            if (kanban == null)
            {
                return NotFound();
            }

            _context.Kanbans.Remove(kanban);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KanbanExists(Guid id)
        {
            return (_context.Kanbans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
