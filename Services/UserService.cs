using CarRent.Interfaces;
using CarRent.DataBase;
using CarRent.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace CarRent.Services;
// TODO: Endpoint wybierania auta do wyporzyczenia
// TODO: endpoint zatwierdzenie płatności
// TODO: endpoint przypisanie auta do użytkownika
// TODO: Endpoint zwrot auta
// TODO: Endpoint Wyświetlenie wszystkich dostępnych aut
// TODO: Endpoint wyświetlanie danych użytkownika oraz jego wyporzyczonych aut
// TODO: Stworzyć klasę view i ją wysyłać
// TODO: Stworzyć automatyczne testy
// TODO: Przerobić metody aby nie zwarały w catchu null tylko informacje wzrotną
public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager,AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<bool> AddAdminToRoleAsync(string id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                Console.WriteLine("user jest nulem");
                return false;
            }
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddAsync(int pesel, string name, string lastName, 
        string password, string email)
    {
        try
        {
            User newUser = new User()
            {
                Pesel = pesel,
                Name = name,
                LastName = lastName,
                Email = email,
                UserName = email,
                Cart = new Cart()
            };
            var result = await _userManager.CreateAsync(newUser, password);
            // foreach (var error in result.Errors)
            // {
            //     // error.Code to nazwa błędu, error.Description to wyjaśnienie
            //     Console.WriteLine($"Kod: {error.Code}, Opis: {error.Description}");
            // }
            return result.Succeeded;   
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string Id)
    {
        try
        {
            var user = await _context.Users.FindAsync(Id);
            if(user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<User> InfoAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<IList<User>> InfoAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<string> LoginAsync(string userName, string password)
    {
        try
        {   
            var user = await _userManager.FindByEmailAsync(userName);
            if(user != null)
            {
                var ifCorrectPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if(ifCorrectPassword.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return _tokenService.CreateToken(user, roles);
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}