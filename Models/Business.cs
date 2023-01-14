#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpliCRM.Models;
public class Business
{
    [Key]
    public int BusinessId {get;set;}
    public string Name {get;set;}
    public int BusinessOwnerId {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // navigation
    public Owner BusinessOwner { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
    public List<Customer> Customers {get; set;} = new List<Customer>();
}
