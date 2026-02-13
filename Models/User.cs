using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
// TODO: sprawdzanie czy pesel ma wystaczajaca liczbe cyfr
// TODO: poprawic testy
// TODO: zrobic testy czy da sie włamać
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