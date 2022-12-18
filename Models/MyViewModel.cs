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
}