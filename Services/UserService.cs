using CarRent.Interfaces;
using CarRent.DataBase;
using CarRent.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace CarRent.Services;
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
                Cart = new Cart(),
                Cars = new List<Car>()
            };
            newUser.Cart.User = newUser;
            var result = await _userManager.CreateAsync(newUser, password);
            foreach (var error in result.Errors)
            {
                // error.Code to nazwa błędu, error.Description to wyjaśnienie
                // TODO: wytnij to
                Console.WriteLine($"Kod: {error.Code}, Opis: {error.Description}");
            }
            result = await _userManager.AddToRoleAsync(newUser, "User");
                        foreach (var error in result.Errors)
            {
                // error.Code to nazwa błędu, error.Description to wyjaśnienie
                Console.WriteLine($"Kod2: {error.Code}, Opis: {error.Description}");
            }
            return result.Succeeded;   
        }
        catch(Exception ex)
        {
            Console.WriteLine("PEŁNY BŁĄD:");
            Console.WriteLine(ex.ToString());
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
                    var cart = await _context.Carts.FirstOrDefaultAsync(c => c.IdUser == user.Id);
                    return _tokenService.CreateToken(user, roles, cart.Id);
                }
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine("PEŁNY BŁĄD:");
            Console.WriteLine(ex.ToString());
            return null;
        }
    }
}