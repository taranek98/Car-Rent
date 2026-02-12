using System.ComponentModel.DataAnnotations;

namespace CarRent.Models;

public class Cart
{
    [Key]
    [Required]
    public string Id {get; set;}
    [Required]
    public string IdUser {get; set;}
    [Required]
    public User User {get; set;}
    public IList<Car>? Cars {get; set;}

}