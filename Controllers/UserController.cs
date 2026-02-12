using Microsoft.AspNetCore.Mvc;
using CarRent.Interfaces;
using CarRent.DataBase;
using CarRent.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace CarRent.Controllers;
public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("User/Register")]
    public async Task<IActionResult> Register(int pesel, string name, string lastName, string password, string email)
    {
        bool result = await _userService.AddAsync(pesel,name, lastName, password, email);
        if(!result)
        {
            return StatusCode(400, "Błąd podczas Rejestracji");
        }
        return Ok("Rejestracja przebiegła pomyślnie");   
    }

    [HttpPost]
    [Route("User/Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var token = await _userService.LoginAsync(email, password);
        if(token != null)
        {
            return Ok(new 
            {
                token = token
            });
        }
        return StatusCode(400, "Błąd podczas logowania");
    }

    [HttpGet]
    [Route("/User/Get")]
    [Authorize]
    public async Task<IActionResult> GetInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.InfoAsync(userId);
        if(user == null)
        {
            return StatusCode(400, "Błąd podczas Pobierania danych");
        }
        return Ok(user);
    }

    [HttpGet]
    [Route("/User/GetAll")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetInfoAll()
    {
        var users = await _userService.InfoAllAsync();
        if(users == null)
        {
            return StatusCode(400, "Błąd podczas Pobierania danych");
        }
        return Ok(users);
    }

    [HttpDelete]
    [Route("User/Delete")]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(! await _userService.DeleteAsync(userId))
        {
            return StatusCode(400, "Nie da się usunąć użytkownika");
        }
        return Ok("użytkownik został usunięty");
    }

    [HttpPost]
    [Route("User/Admin/Assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminAssign(string id)
    {
        if(! await _userService.AddAdminToRoleAsync(id))
        {
            return StatusCode(400, "Nie da się nadać roli admina");
        }
        return Ok("użytkownik został adminem");
    }
}