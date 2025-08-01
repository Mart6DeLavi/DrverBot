using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CargoDesk.API.Controllers;

[ApiController]
[Route("api/v1/routes")]
public class RouteController : ControllerBase
{
    private readonly IRouteService _routeService;

    public RouteController(IRouteService routeService)
    {
        _routeService = routeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteEntity>>> GetAllRoutes()
    {
        var list = await _routeService.GetAllRoutes();
        return Ok(list);
    }

    [HttpGet("{routeId:guid}")]
    public async Task<ActionResult<RouteEntity>> GetRouteById(Guid routeId, CancellationToken ct)
    {
        try
        {
            var route = await _routeService.GetRouteById(routeId, ct);
            return Ok(route);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<RouteEntity>> CreateNewRoute([FromBody] CreateNewRouteDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _routeService.CreateNewRoute(dto, ct);

        return CreatedAtAction(
            nameof(GetRouteById),
            new { routeId = created.Id },
            created
        );
    }

    [HttpDelete("{routeId:guid}")]
    public async Task<IActionResult> DeleteRouteById(Guid routeId)
    {
        try
        {
            await _routeService.DeleteRouteById(routeId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}