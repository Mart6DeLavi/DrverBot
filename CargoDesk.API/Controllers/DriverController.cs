using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CargoDesk.API.Controllers;

[ApiController]
[Route("api/v1/drivers")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriverController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<DriverEntity>>> GetAllDrivers()
    {
        var list = await _driverService.GetAllDrivers();
        return Ok(list);
    }

    [HttpGet("{driverId:guid}")]
    public async Task<ActionResult<DriverEntity>> GetDriverById(Guid driverId)
    {
        try
        {
            var driver = await _driverService.GetDriverById(driverId);
            return Ok(driver);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<DriverEntity>> CreateNewDriver([FromBody] CreateNewDriverDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _driverService.CreateNewDriver(dto);
        return CreatedAtAction(
            nameof(GetDriverById),
            new { driverId = created.Id },
            created
        );
    }

    [HttpDelete("{driverId:guid}")]
    public async Task<IActionResult> DeleteDriverById(Guid driverId)
    {
        try
        {
            await _driverService.DeleteDriverById(driverId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}