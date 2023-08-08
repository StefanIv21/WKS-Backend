using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WKS1.Models;

namespace WKS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserItemsController : ControllerBase
    {
        private readonly UserContext _context;

        public UserItemsController(UserContext context)
        {
            _context = context;
        }

        // GET: api/UserItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetUserItems()
        {
          if (_context.UserItems == null)
          {
              return NotFound();
          }
            return await _context.UserItems.Select(x=> ItemtoDto(x)).ToListAsync();
        }

        // GET: api/UserItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItemDto>> GetUserItem(int id)
        {
          if (_context.UserItems == null)
          {
              return NotFound();
          }
            var userItem = await _context.UserItems.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            return Ok(ItemtoDto(userItem));
        }

        // PUT: api/UserItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(int id, UserItemDto userItem)
        {
            if (id != userItem.Id)
            {
                return BadRequest();
            }
            var userItemSet = await _context.UserItems.FindAsync(id);
            _context.Entry(userItemSet).State = EntityState.Modified;

            userItemSet.Nume = userItem.Nume;
            userItemSet.Prenume = userItem.Prenume;
            userItemSet.Email = userItem.Email;
            userItemSet.NumarTelefon = userItem.NumarTelefon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
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

        // POST: api/UserItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserItemDto>> PostUserItem(UserItemDto userItemDto)
        {
          if (_context.UserItems == null)
          {
              return Problem("Entity set 'UserContext.UserItems'  is null.");
          }

            UserItem userItem = new UserItem
            {
                Nume = userItemDto.Nume,
                Email = userItemDto.Email,
                Prenume = userItemDto.Prenume,
                NumarTelefon = userItemDto.NumarTelefon,
            };
            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserItem", new { id = userItem.Id }, userItem);
        }

        // DELETE: api/UserItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItem(int id)
        {
            if (_context.UserItems == null)
            {
                return NotFound();
            }
            var userItem = await _context.UserItems.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            _context.UserItems.Remove(userItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserItemExists(int id)
        {
            return (_context.UserItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private static UserItemDto ItemtoDto(UserItem userItem)
        {
            return new UserItemDto
            {
                Id = userItem.Id,
                Nume = userItem.Nume,
                Prenume = userItem.Prenume,
                Email = userItem.Email,
                NumarTelefon = userItem.NumarTelefon,


            };
        }
    }
}
