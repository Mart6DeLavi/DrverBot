using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CargoDesk.API.Controllers;

[ApiController]
[Route("api/v1/addresses")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AddressEntity>>> GetAllAddresses()
    {
        var list = await _addressService.GetAllAddresses();
        return Ok(list);
    }

    [HttpGet("{addressId:guid}")]
    public async Task<ActionResult<AddressEntity>> GetAddressById(Guid addressId)
    {
        try
        {
            var address = await _addressService.GetAddressById(addressId);
            return Ok(address);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<AddressEntity>> CreateNewAddress([FromBody] CreateNewAddressDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _addressService.CreateNewAddress(dto);

        return CreatedAtAction(
            nameof(GetAddressById),
            new { addressId = created.Id },
            created
        );
    }

    [HttpDelete("{addressId:guid}")]
    public async Task<IActionResult> DeleteAddressById(Guid addressId)
    {
        try
        {
            await _addressService.DeleteAddressById(addressId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}