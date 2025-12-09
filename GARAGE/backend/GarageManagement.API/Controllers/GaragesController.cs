using GarageManagement.API.Data;
using GarageManagement.API.Models;
using GarageManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GaragesController : ControllerBase
{
    private readonly GarageDbContext _context;
    private readonly GovernmentApiService _governmentApiService;

    public GaragesController(GarageDbContext context, GovernmentApiService governmentApiService)
    {
        _context = context;
        _governmentApiService = governmentApiService;
    }

    [HttpGet("from-government")]
    public async Task<ActionResult<List<Garage>>> GetGaragesFromGovernment()
    {
        try
        {
            var garages = await _governmentApiService.GetGaragesFromGovernmentApiAsync();
            return Ok(garages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching garages from government API: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<Garage>>> GetGarages()
    {
        try
        {
            var garages = await _context.Garages.ToListAsync();
            return Ok(garages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving garages: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Garage>> AddGarage(Garage garage)
    {
        if (garage == null)
        {
            return BadRequest("Garage data is required");
        }

        try
        {
            if (!string.IsNullOrEmpty(garage.GovernmentId))
            {
                var exists = await _context.Garages
                    .AnyAsync(g => g.GovernmentId == garage.GovernmentId);
                if (exists)
                {
                    return Conflict("Garage with this Government ID already exists");
                }
            }

            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGarages), new { id = garage.Id }, garage);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding garage: {ex.Message}");
        }
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<List<Garage>>> AddGaragesBulk(List<Garage> garages)
    {
        if (garages == null || garages.Count == 0)
        {
            return BadRequest("Garages data is required");
        }

        try
        {
            var existingIds = await _context.Garages
                .Where(g => g.GovernmentId != null)
                .Select(g => g.GovernmentId)
                .ToListAsync();

            var toAdd = garages
                .Where(g => g.GovernmentId == null || !existingIds.Contains(g.GovernmentId))
                .ToList();

            if (toAdd.Count == 0)
            {
                return Ok(new List<Garage>());
            }

            _context.Garages.AddRange(toAdd);
            await _context.SaveChangesAsync();

            return Ok(toAdd);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding garages: {ex.Message}");
        }
    }
}
