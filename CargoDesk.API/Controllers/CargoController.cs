using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CargoDesk.API.Controllers;

[ApiController]
[Route("api/v1/cargos")]
public class CargoController : ControllerBase
{
    private readonly ICargoService _cargoService;

    public CargoController(ICargoService cargoService)
    {
        _cargoService = cargoService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<CargoEntity>>> GetAllCargos()
    {
        var list = await _cargoService.GetAllCargos();
        return Ok(list);
    }

    [HttpGet("{cargoId:guid}")]
    public async Task<ActionResult<CargoEntity>> GetCargoById(Guid cargoId, CancellationToken ct)
    {
        try
        {
            var cargo = await _cargoService.GetCargoById(cargoId, ct);
            return Ok(cargo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<CargoEntity>> CreateNewCargo([FromBody] CreateNewCargoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _cargoService.CreateNewCargo(dto);

        return CreatedAtAction(
            nameof(GetCargoById),
            new { cargoId = created.Id },
            created
        );
    }

    [HttpDelete("{cargoId:guid}")]
    public async Task<IActionResult> DeleteCargoById(Guid cargoId)
    {
        try
        {
            await _cargoService.DeleteCargoById(cargoId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}