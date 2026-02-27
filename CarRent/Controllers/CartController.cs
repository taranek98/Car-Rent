using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRent.Interfaces;
using System.Security.Claims;
using CarRent.Models;

namespace CarRent.Controllers;
public class CartController : Controller
{
    private readonly ICartInterface _cartService;
    public CartController(ICartInterface cartInterface)
    {
        _cartService = cartInterface;
    }
    [HttpPost]
    [Route("Cart/Add")]
    [Authorize]
    public async Task<IActionResult> AddCar(string vin)
    {
        var cartId = User.FindFirstValue("CartId") ?? string.Empty;
        var result = await _cartService.AddCarAsync(vin, cartId)!;
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }

    [HttpGet]
    [Route("Cart/Get")]
    [Authorize]
    public async Task<IActionResult> GetCars()
    {
        var idCart = User.FindFirstValue("CartId") ?? string.Empty;
        var result = await _cartService.GetCarsAsync(idCart);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);
    }
    [HttpPost]
    [Route("Cart/Buy")]
    [Authorize]
    public async Task<IActionResult> BuyCars()
    {
        var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var idCart = User.FindFirstValue("CartId") ?? string.Empty;
        var result = await _cartService.BuyCarsAsync(idUser, idCart);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result.Message);
    }
}