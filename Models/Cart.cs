using System.ComponentModel.DataAnnotations;

namespace CarRent.Models;

public class Cart
{
    [Key]
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string? IdUser {get; set;}
    public User? User {get; set;}
    public IList<Car> Cars {get; set;} = new List<Car>();
}