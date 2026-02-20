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
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Create(string VIN, string Mark, string Model, 
        string Color, string fuel, float Prize)
    {
        if(! await _carService.AddAsync(VIN, Mark, Model, Color, fuel, Prize))
        {
            return StatusCode(400, "Błąd dodawania nowego samochodu");
        }
        return Ok();
    }

    [HttpGet]
    [Route("Car/Get")]
    [Authorize]
    public async Task<IActionResult> GetAllAvailable()
    {
        var list = await _carService.InfoAllAvailableAsync();
        if(list == null)
        {
            return StatusCode(400, "Bład wczytywania");
        }
        return Ok(list);
    }

    [HttpDelete]
    [Route("Car/Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string VIN)
    {
        if(! await _carService.DeleteAsync(VIN))
        {
            return StatusCode(400, "Błąd przy usuwaniu");
        }
        return Ok("Element usunięty");
    }
}