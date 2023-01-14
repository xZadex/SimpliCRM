#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace SimpliCRM.Models;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
}