using Microsoft.AspNetCore.Mvc;
using CarRent.Interfaces;
using CarRent.Models;
using Microsoft.AspNetCore.Authorization;
namespace CarRent.Controllers;


public class CarController : Controller
{
    private readonly ICarService _carService;
    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpPost]
    [Route("Car/Create")]
    // TODO: Po testach zmienić User na Admin
    [Authorize(Roles ="User")]
    public async Task<IActionResult> Create(string VIN, string Mark, string Model, 
        string Color, string fuel, decimal Prize)
    {
        var result = await _carService.AddAsync(VIN, Mark, Model, Color, fuel, Prize);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }

    [HttpGet]
    [Route("Car/Get")]
    [Authorize]
    public async Task<IActionResult> GetAllAvailable()
    {
        var result = await _carService.InfoAllAvailableAsync();
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);
    }

    [HttpDelete]
    [Route("Car/Delete")]
    // TODO: Po testach zmienić User na Admin
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Delete(string VIN)
    {
        var result = await _carService.DeleteAsync(VIN);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }
}