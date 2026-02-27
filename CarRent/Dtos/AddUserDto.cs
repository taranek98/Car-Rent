using System.ComponentModel.DataAnnotations;

namespace CarRent.Dtos;
public class AddUserDto
{
    [Required(ErrorMessage = "Pesel jest wymagany")]
    public int Pesel {get; set;}

    [Required(ErrorMessage = "Imię jest wymagane")]
    public required string Name {get; set;}

    [Required(ErrorMessage = "Nazwisko jest jest wymagany")]
    public required string LastName {get; set;}

    [Required(ErrorMessage = "Hasło jest wymagany")]
    public required string Password {get; set;}

    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress]
    public required string Email {get; set;}
    
}