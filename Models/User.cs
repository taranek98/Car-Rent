using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace CarRent.Models;
public class User : IdentityUser
{   
    [Required]
    public long Pesel {get; set;}
    [Required]
    public required string Name {get; set;}
    [Required]
    public required string LastName {get; set;}
    public  IList<Car>? Cars {get; set;}
    public Cart? Cart{get; set;}
}