#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpliCRM.Models;
public class Employee
{
    [Key]
    public int EmployeeId {get;set;}

    [Required(ErrorMessage ="First Name Required*")]
    public string FirstName {get;set;}

    [Required(ErrorMessage ="Last Name Required*")]
    public string LastName {get;set;}

    [Required(ErrorMessage ="Email Required*")]
    [EmailAddress]
    [UniqueEmployee]
    public string Email {get;set;}

    [Required(ErrorMessage ="Role Required*")]
    public string Role {get;set;}

    [Required(ErrorMessage ="Birthday Required*")]
    [DataType(DataType.Date)]
    public DateTime Birthday {get;set;}

    [Required(ErrorMessage ="Password Required*")]
    [DataType(DataType.Password)]
    [MinLength(8,ErrorMessage ="Password must be at least 8 characters.")]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage="Confirm is required*")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }
    public int BusinessId {get;set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // navigation
    public Business Company { get; set; }
}

public class UniqueEmployeeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Email is required*");
        }

        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if(_context.Employees.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique*");
        } else {
            return ValidationResult.Success;
        }
    }
}