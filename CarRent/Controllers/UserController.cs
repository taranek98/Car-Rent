using Microsoft.AspNetCore.Mvc;
using CarRent.Interfaces;
using CarRent.DataBase;
using CarRent.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarRent.Dtos;
namespace CarRent.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("User/Register")]
    public async Task<IActionResult> Register([FromBody]AddUserDto dto)
    {
        var result = await _userService.AddAsync(dto.Pesel,dto.Name, dto.LastName, dto.Password, dto.Email);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);   
    }

    [HttpPost]
    [Route("User/Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _userService.LoginAsync(email, password);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(new 
        {
            token = result.Data
        });
    }

    [HttpGet]
    [Route("/User/Get")]
    [Authorize]
    public async Task<IActionResult> GetInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var result = await _userService.InfoAsync(userId);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);  
    }

    [HttpGet]
    [Route("/User/GetAll")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetInfoAll()
    {
        var result = await _userService.InfoAllAsync();
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);  
    }

    [HttpDelete]
    [Route("User/Delete")]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var result =  await _userService.DeleteAsync(userId);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);  
    }

    [HttpPost]
    [Route("User/Admin/Assign")]
    // TODO: Po testach zmienić User na Admin
    [Authorize(Roles = "User")]
    public async Task<IActionResult> AdminAssign(string id)
    {
        var result = await _userService.AddAdminToRoleAsync(id);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);  
    }

    [HttpPost]
    [Route("User/Add/Funds")]
    [Authorize]
    public async Task<IActionResult> AddFunds(decimal funds)
    {
        var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var result = await _userService.AddFundsAsync(idUser, funds);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);  
    }
}