using managementapp.Data;
using managementapp.Data.DTO;
using managementapp.Data.Models;
using managementapp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace managementapp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodolistsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IDataService _dataService;

    public TodolistsController(DataContext context, IDataService data)
    {
        _context = context;
        _dataService = data;
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
        string token = HttpContext.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(token))
        {
            // Handle the case where the token is null or empty
            return Unauthorized(); // Or another appropriate action
        }
        Token tokenData = _dataService.DeToken(token);

        Guid userId = Guid.Parse(tokenData.Id.ToString());

        var newTodo = new Todolist
        {
            Id = Guid.NewGuid(),
            Title = todolist.Title,
            Description = todolist.Description,
            Status = todolist.Status,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserId = userId,
            ProjectId = todolist.ProjectId
        };
        await _context.AddAsync(newTodo);
        await _context.SaveChangesAsync();

        return Ok(newTodo.Id);
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

