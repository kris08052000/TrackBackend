using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRACK_BACKEND.Data;
using TRACK_BACKEND.Models;

namespace TRACK_BACKEND.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ItemsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/items
    [HttpGet]
    public async Task<IActionResult> GetItem()
    {
        try
        {
            var item = await _context.Items.ToListAsync();
            return Ok(new { Status = 200, data = item });
        } catch (Exception ex)
        {
            return StatusCode(500, new { status = 500, error = ex.Message,  inner = ex.InnerException?.Message });
        }
    }

    // GET: api/items
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        try
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound(new { status = 404, message = "Item not found" });
            return Ok(new { Status = 200, data = item });
        }catch(Exception ex)
        {
            return StatusCode(500, new { status = 500, error = ex.Message,  inner = ex.InnerException?.Message });
        }

    
    }

    // POST: api/items
    [HttpPost]
    public async Task<IActionResult> CreateItem(Item item)
    {
        try
        {
            item.UpdatedAt = DateTime.UtcNow;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                status = 201,
                message = "Item created successfully",
                data = item
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = 500, error = ex.Message,  inner = ex.InnerException?.Message });
        }
    }

    // PUT: api/items/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, Item update)
    {
        if (id != update.Id)
        {
            return BadRequest(new { status = 400, message = "ID mismatch" });
        }
        try
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound(new { status = 404, message = "Item not found" });

            item.Name = update.Name;
            item.Location = update.Location;
            item.Price = update.Price;
            item.Latitude = update.Latitude;
            item.Longitude = update.Longitude;
            item.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                status = 200,
                message = "Item updated successfully",
                data = item
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = 500, error = ex.Message,  inner = ex.InnerException?.Message });
        }
    }

    // DELETE: api/items/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        try
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound(new { status = 404, message = "Item not found" });
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return StatusCode(204, new { status = 204, message = "Item deleted" });
        }catch(Exception ex)
        {
            return StatusCode(500, new { status = 500, error = ex.Message,  inner = ex.InnerException?.Message });
        }
    }
}