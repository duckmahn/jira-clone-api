using managementapp.Data;
using managementapp.Data.DTO;
using managementapp.Data.Models;
using managementapp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace managementapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDataService _dataService;

        public ProjectsController(DataContext context , IDataService dataService)
        {
            _context = context;
            _dataService  = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
          var projects = await _context.Projects.ToListAsync();
            return Ok(projects); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            var project = await _context.Projects
                .Include(project => project.ProjectUsers)
                .ToListAsync();

            project = project.FindAll(project => project.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(Guid id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        public async Task<ActionResult<Project>> CreateProjectWithDefaultKanbans(ProjectDTO projectDto)
        {
            // Create the project
            var project = new Project
            {
                Title = projectDto.Title,
                AdminId = _dataService.GetTokenData().Id
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Create default Kanban boards
            var defaultKanbans = new List<Kanban>
            {
                new Kanban { Name = "Backlog", StatusList = "1", ProjectId = project.Id },
                new Kanban { Name = "In Process", StatusList = "2", ProjectId = project.Id },
                new Kanban { Name = "Done", StatusList = "3", ProjectId = project.Id }
            };

            foreach (var kanban in defaultKanbans)
            {
                _context.Kanbans.Add(kanban);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }


        [HttpPut("AddUserToProject")]
        public async Task<ActionResult> AddUserToProject(Guid projectId, Guid userIdToAdd)
        {
            // Get the current user's ID from the token or context
            var currentUserId = _dataService.GetTokenData().Id; // Assuming this method returns the current user's details including Id

            // Check if the project exists and get the admin ID
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Check if the current user is the admin of the project
            if (project.AdminId != currentUserId)
            {   
                return Forbid("Only the project's admin can add users to the project");
            }

            // Check if the admin is trying to add themselves
            if (userIdToAdd == project.AdminId)
            {
                return BadRequest("Admin is already part of the project");
            }

            // Check if the user to add exists
            var userToAdd = await _context.Users.FindAsync(userIdToAdd);
            if (userToAdd == null)
            {
                return NotFound("User to add not found");
            }

            // Check if the user is already added to the project
            var existingProjectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userIdToAdd);
            if (existingProjectUser != null)
            {
                return BadRequest("User already added to the project");
            }

            // Create a new ProjectUser relationship
            var projectUser = new ProjectUser
            {
                ProjectId = projectId,
                UserId = userIdToAdd
            };

            _context.ProjectUsers.Add(projectUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("RemoveUserFromProject")]
        public async Task<ActionResult> RemoveUserFromProject(Guid projectId, Guid userIdToRemove)
        {
            // Get the current user's ID from the token or context
            var currentUserId = _dataService.GetTokenData().Id; // Assuming this method returns the current user's details including Id

            // Check if the project exists
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Check if the current user is the admin of the project
            if (project.AdminId != currentUserId)
            {
                return Forbid("Only the project's admin can remove users from the project");
            }

            // Check if the admin is trying to remove themselves
            if (userIdToRemove == project.AdminId)
            {
                return BadRequest("Admin cannot remove themselves from the project");
            }

            // Find the ProjectUser relationship to remove
            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userIdToRemove);
            if (projectUser == null)
            {
                return NotFound("User not found in the project");
            }

            // Remove the ProjectUser relationship
            _context.ProjectUsers.Remove(projectUser);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(Guid id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string GetUserId()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString();
            return _dataService.GetUserId(token);
        }
    }
}
