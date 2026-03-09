using System.ComponentModel.DataAnnotations;

namespace CarRent.Dtos;
public class AddUserDto
{
    [Required(ErrorMessage = "Pesel jest wymagany")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Numer Pesel jest niepoprawny")]
    public required string Pesel {get; set;}

    [Required(ErrorMessage = "Imię jest wymagane")]
    public required string Name {get; set;}

    [Required(ErrorMessage = "Nazwisko jest jest wymagane")]
    public required string LastName {get; set;}

    [Required(ErrorMessage = "Hasło jest wymagane")]
    public required string Password {get; set;}

    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress]
    public required string Email {get; set;}
    
}