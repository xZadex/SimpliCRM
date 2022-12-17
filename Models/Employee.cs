#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpliCRM.Models;
public class Employee
{
    [Key]
    public int EmployeeId {get;set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string Email {get;set;}
    public DateTime Birthday {get;set;}
    public string Role {get;set;}

    [DataType(DataType.Password)]
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // navigation
    public Business Company { get; set; }
}