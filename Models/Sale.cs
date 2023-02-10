#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpliCRM.Models;
public class Sale
{
    [Key]
    public int SaleId {get;set;}
    public string CustomerName {get;set;}
    public int BusinessId{get; set;}
    public string Product {get; set;}
    public double Cost {get; set;}
    public string Status {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // navigation
    public Business Company {get; set;}
}
