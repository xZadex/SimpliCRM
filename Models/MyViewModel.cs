#pragma warning disable CS8618
namespace SimpliCRM.Models;
public class MyViewModel
{
    public Owner? Owner {get;set;}
    public List<Owner>? AllOwners {get;set;}

    public Business? Business {get;set;}
    public List<Business>? AllBusinesses {get;set;}

    public Employee? Employee {get;set;}
    public List<Employee>? AllEmployees {get;set;}

    public Customer? Customer {get;set;}
    public List<Customer>? AllCustomers {get;set;}

    public Sale? Sale {get; set;}
    public List<Sale>? AllSales {get; set;}
}