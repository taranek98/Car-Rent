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
        var cartId = User.FindFirstValue("CartId");
        if(cartId == null)
        {
            return StatusCode(400, "Coś poszło nie tak");
        }
        if(! await _cartService.AddCar(vin, cartId))
        {
            return StatusCode(400, "Coś poszło nie tak");
        }
        return Ok("Auto zostało dodane do koszyka");
    }

    [HttpGet]
    [Route("Cart/Get")]
    [Authorize]
    public async Task<IActionResult> GetCars()
    {
        var idCart = User.FindFirstValue("CartId");
        var list = await _cartService.GetCars(idCart);
        if(list == null)
        {
            return StatusCode(500, "Coś poszło nie tak");
        }
        return Ok(list);
    }
}