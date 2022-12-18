#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpliCRM.Models;
public class Owner
{
    [Key]
    public int OwnerId {get;set;}

    [Required(ErrorMessage="First Name is required*")]
    [MinLength(2, ErrorMessage ="Minimum 2 characters*")]
    public string FirstName { get; set; }

    [Required(ErrorMessage="Last Name is required*")]
    [MinLength(2, ErrorMessage ="Minimum 2 characters*")]
    public string LastName { get; set; }

    [Required(ErrorMessage="Email is required*")]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required(ErrorMessage="Password is required*")]
    [MinLength(8, ErrorMessage ="Minimum of 8 characters*")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage="Confirm password is required*")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // navigation
    public List<Business> CreatedBusinesses { get; set; } = new List<Business>();
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Email is required*");
        }

        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if(_context.Owners.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique*");
        } else {
            return ValidationResult.Success;
        }
    }
}

// public class UniqueBusinessAttribute : ValidationAttribute
// {
//     protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
//     {
//         if(value == null)
//         {
//             return new ValidationResult("Business name is required*");
//         }

//         MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
//         if(_context.Owners.Any(e => e.BusinessName == value.ToString()))
//         {
//             return new ValidationResult("This business is already registered*");
//         } else {
//             return ValidationResult.Success;
//         }
//     }
// }