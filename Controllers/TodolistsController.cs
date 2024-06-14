using managementapp.Data;
using managementapp.Data.DTO;
using managementapp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace managementapp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodolistsController : ControllerBase
{
    private readonly DataContext _context;

    public TodolistsController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Todolists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todolist>>> GetTodolists()
    {
        if (_context.Todolists == null)
        {
            return NotFound();
        }
        return await _context.Todolists.ToListAsync();
    }

    // GET: api/Todolists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Todolist>> GetTodolist(int id)
    {
        if (_context.Todolists == null)
        {
            return NotFound();
        }
        var todolist = await _context.Todolists.FindAsync(id);

        if (todolist == null)
        {
            return NotFound();
        }

        return todolist;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodolist(Guid id, Todolist todolist)
    {
        if (id != todolist.Id)
        {
            return BadRequest();
        }

        _context.Entry(todolist).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodolistExists(id))
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
    public async Task<ActionResult<Todolist>> PostTodolist(TodoDTO todolist)
    {
        if (_context.Todolists == null)
        {
            return Problem("Entity set 'Dbcontext.Todolists'  is null.");
        }

        _context.Todolists.Add(new Todolist
        {
            Title = todolist.Title,
            Description = todolist.Description,
            Status = todolist.Status,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        });
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTodolist", todolist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodolist(int id)
    {
        if (_context.Todolists == null)
        {
            return NotFound();
        }
        var todolist = await _context.Todolists.FindAsync(id);
        if (todolist == null)
        {
            return NotFound();
        }

        _context.Todolists.Remove(todolist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodolistExists(Guid id)
    {
        return (_context.Todolists?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

