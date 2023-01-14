#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpliCRM.Models;
public class Customer
{
    [Key]
    public int CustomerId {get;set;}
    public int BusinessId {get;set;}

    [Required(ErrorMessage ="First Name Required*")]
    public string FirstName {get;set;}

    [Required(ErrorMessage ="Last Name Required*")]
    public string LastName {get;set;}

    [Required(ErrorMessage ="Phone Number Required*")]
    public string PhoneNumber {get;set;}

    [Required(ErrorMessage ="Email Required*")]
    [EmailAddress]
    public string Email {get;set;}

    [DataType(DataType.Date)]
    public DateTime Birthday {get;set;}

    [Required(ErrorMessage = "Select Status*")]
    public string Status {get;set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // navigation
    public Business Company { get; set; }
}