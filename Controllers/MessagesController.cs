using managementapp.Data;
using managementapp.Data.DTO;
using managementapp.Data.Models;
using managementapp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace managementapp.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHubContext<SignalHub> _hubContext;
        

        public MessagesController(DataContext context , IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
            _context = context;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(Guid id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage( MessageDTO messageDTO)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var recieverId = messageDTO.RecieverId;
            Guid.TryParse(currentUserId, out Guid currentUserIdGuid);
            Guid.TryParse(recieverId, out Guid recieverGuid);

            if (_context.Messages == null)
            {
                return Problem("Entity set 'DataContext.Message'  is null.");
            }
            var chatMessage = new Message
            {
                Id = Guid.NewGuid(),
                MessageText = messageDTO.MessageText,
                TimeStamp = DateTime.Now,
                SenderId = currentUserIdGuid,
                RecieverId = recieverGuid,
            };

            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", chatMessage);

            return Ok( chatMessage );
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(Guid id)
        {
            return (_context.Messages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
