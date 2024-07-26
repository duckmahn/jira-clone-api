using managementapp.Data;
using managementapp.Data.DTO;
using managementapp.Data.Models;
using managementapp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
    public async Task<ActionResult<IEnumerable<Todolist>>> GetTodolists(DateTime? date)
    {
        if (_context.Todolists == null)
        {
            return NotFound();
        }

        IQueryable<Todolist> query = _context.Todolists;

        if (date.HasValue)
        {
            // Assuming you want to filter Todolists where the CreatedAt, UpdatedAt, or ExpiredAt matches the date
            // Adjust the filtering logic based on your specific requirements
            query = query.Where(t => (t.ExpiredAt  == date.Value.Date));
        }

        return await query.ToListAsync();
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<Todolist>>> GetTodolistsByProjectId(Guid projectId)
    {
        if (_context.Todolists == null)
        {
            return NotFound();
        }

        return Ok(await _context.Todolists.Where(x => x.ProjectId == projectId).ToListAsync());
    }
    // GET: api/Todolists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Todolist>> GetTodolist(Guid id)
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
    public async Task<IActionResult> PutTodolist(Guid id, TodoUpdateDTO todolist)
    {
        var todo = await _context.Todolists.FindAsync(id);

        todo.Title = todolist.Title;
        todo.Description = todolist.Description;
        todo.CreatedAt = todo.CreatedAt;
        todo.UpdatedAt = DateTime.Now;
        todo.UserId = todo.UserId;
        todo.ProjectId = todo.ProjectId;
        todo.ExpiredAt = todolist.ExpiredAt;
        todo.Status = todolist.Status;
        todo.StatusName = todolist.StatusName;


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

    [HttpPut("{id}/UpdateStatus")]
    public async Task<IActionResult> UpdateStatus(Guid id, int status)
    {
        var todo = await _context.Todolists.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }

        var kanban = await _context.Kanbans
            .Where(k => k.ProjectId == todo.ProjectId && k.Status == status)
            .FirstOrDefaultAsync();

        if (kanban == null)
        {
            return NotFound("Kanban status not found for the project");
        }

        todo.Status = status;
        todo.StatusName = kanban.StatusName;
        todo.UpdatedAt = DateTime.Now;

        _context.Entry(todo).State = EntityState.Modified;

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
    #region Postnormal
    //[HttpPost]
    //public async Task<ActionResult<Todolist>> PostTodolist(TodoDTO todolist)
    //{
    //    string token = HttpContext.Request.Headers["Authorization"];
    //    if (string.IsNullOrEmpty(token))
    //    {
    //        // Handle the case where the token is null or empty
    //        return Unauthorized(); // Or another appropriate action
    //    }
    //    Token tokenData = _dataService.DeToken(token);

    //    Guid userId = Guid.Parse(tokenData.Id.ToString());

    //var newTodo = new Todolist
    //{
    //    Id = Guid.NewGuid(),
    //    Title = todolist.Title,
    //    Description = todolist.Description,
    //    Status = 1,

    //    CreatedAt = DateTime.Now,
    //    UpdatedAt = DateTime.Now,
    //    UserId = userId,
    //    ProjectId = todolist.ProjectId,
    //    StatusName = _context.Kanbans
    //        .Where(k => k.ProjectId == todolist.ProjectId && k.Status == 1)
    //        .FirstOrDefault().ToString(),
    //    ExpiredAt = todolist.ExpiredAt
    //};
    //    await _context.AddAsync(newTodo);
    //    await _context.SaveChangesAsync();

    //    return Ok(newTodo.Id);
    //}
    #endregion

    [HttpPost("{projectId}")]
    public async Task<ActionResult<Todolist>> AddTodolist(Guid projectId, TodoDTO todolist)
    {
        // Check if the project exists
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);
        if (!projectExists)
        {
            return NotFound("Project not found");
        }

        // Fetch the default Kanban status for the project
        var defaultKanban = await _context.Kanbans
            .Where(k => k.ProjectId == projectId && k.Status == 1)
            .FirstOrDefaultAsync();

        if (defaultKanban == null)
        {
            return NotFound("Default Kanban status not found for the project");
        }

        var newTodo = new Todolist
        {
            Id = Guid.NewGuid(),
            Title = todolist.Title,
            Description = todolist.Description,
            Status = defaultKanban.Status,
            StatusName = defaultKanban.StatusName,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserId = todolist.userId,
            ProjectId = projectId,
            ExpiredAt = todolist.ExpiredAt
        };

        // Add the new Todolist to the context and save changes
        _context.Todolists.Add(newTodo);
        await _context.SaveChangesAsync();

        return Ok( newTodo);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodolist(Guid id)
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

