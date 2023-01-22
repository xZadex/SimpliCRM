using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpliCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
namespace SimpliCRM.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("owners/create")]
    public IActionResult CreateOwner(Owner newOwner)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<Owner> Hasher = new PasswordHasher<Owner>();
            newOwner.Password = Hasher.HashPassword(newOwner, newOwner.Password);
            _context.Add(newOwner);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("OwnerId", newOwner.OwnerId);
            return RedirectToAction("BusinessSelect");
        }
        else
        {
            return View("Index");
        }
    }

    [HttpGet("businesses/select")]
    public IActionResult BusinessSelect()
    {

        MyViewModel MyModel = new MyViewModel
        {
            Owner = _context.Owners.Include(e => e.CreatedBusinesses).ThenInclude(e => e.Employees).FirstOrDefault(u => u.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
        };
        return View(MyModel);
    }

    [HttpPost("businesses/create")]
    public IActionResult CreateBusiness(Business newBusiness)
    {
        newBusiness.BusinessOwnerId = (int)HttpContext.Session.GetInt32("OwnerId");
        _context.Add(newBusiness);
        _context.SaveChanges();
        return RedirectToAction("BusinessSelect");
    }


    [HttpGet("{name}/Dashboard")]
    public IActionResult Dashboard(string name)
    {
        var ownerId = HttpContext.Session.GetInt32("OwnerId");
        var employeeId = HttpContext.Session.GetInt32("EmployeeId");

        if (ownerId != null)
        {
            var business = _context.Businesses.FirstOrDefault(e => e.BusinessOwnerId == ownerId && e.Name == name);

            if (business == null)
            {
                return RedirectToAction("BusinessSelect");
            }

            HttpContext.Session.SetString("BusinessName", name);
            HttpContext.Session.SetInt32("BusinessId", business.BusinessId);

            Owner? owner = _context.Owners.FirstOrDefault(e => e.OwnerId == ownerId);

            MyViewModel model = new MyViewModel
            {
                Business = business,
                Owner = owner
            };

            return View(model);
        }
        else if (employeeId != null)
        {
            Business? business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(b => b.Name == name);
            Employee? employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == employeeId);

            var model = new MyViewModel
            {
                Business = business,
                Employee = employee
            };
            HttpContext.Session.SetString("BusinessName", name);
            HttpContext.Session.SetInt32("BusinessId", business.BusinessId);
            return View(model);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }



    [HttpGet("{name}/Team")]
    public IActionResult Team(string name)
    {
        if (HttpContext.Session.GetInt32("OwnerId") != null)
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
            };
            return View(MyModel);
        }
        else
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId"))
            };
            return View(MyModel);
        }
    }


    [HttpGet("{name}/Customers")]
    public IActionResult Customers(string name)
    {
        Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

        MyViewModel MyModel = new MyViewModel
        {
            Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
            Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId")),
            Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId")),
            AllCustomers = _context.Customers.Where(e => e.BusinessId == biz.BusinessId).ToList()
        };
        return View(MyModel);
    }


    [HttpGet("{name}/Team/New")]
    public IActionResult NewEmployee(string name)
    {
        if (HttpContext.Session.GetInt32("OwnerId") != null)
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
            };

            return View(MyModel);
        }
        else
        {
            return RedirectToAction("Dashboard");
        }
    }

    [HttpGet("{name}/Customer/New")]
    public IActionResult NewCustomer(string name)
    {
        Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

        MyViewModel MyModel = new MyViewModel
        {
            Business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
            Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId")),
            Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId"))
        };

        return View(MyModel);
    }

    [HttpPost("{name}/Customer/Create")]
    public IActionResult CreateCustomer(string name, Customer newCustomer)
    {
        // if (ModelState.IsValid)
        // {
            newCustomer.BusinessId = (int)HttpContext.Session.GetInt32("BusinessId");
            _context.Add(newCustomer);
            _context.SaveChanges();
            return RedirectToAction("Customers", new { name });
        // }
        // else
        // {
        //     Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

        //     MyViewModel MyModel = new MyViewModel
        //     {
        //         Business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
        //         Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId")),
        //         Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId"))
        //     };
        //     return View("NewCustomer", MyModel);
        // }
    }


    [HttpPost("{name}/Customer/{id}/update")]
    public IActionResult UpdateCustomer(string name, Customer customer, int id)
    {
        Customer? CustomerToUpdate = _context.Customers.FirstOrDefault(d => d.CustomerId == id);
        // if(CustomerToUpdate == null)
        // {
        //     return View("Index");
        // }
        
        // if(ModelState.IsValid)
        // {
            CustomerToUpdate.FirstName = customer.FirstName;
            CustomerToUpdate.LastName = customer.LastName;
            CustomerToUpdate.Email = customer.Email;
            CustomerToUpdate.PhoneNumber = customer.PhoneNumber;
            CustomerToUpdate.Birthday = customer.Birthday;
            CustomerToUpdate.Status = customer.Status;
            CustomerToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Customers", new { name });
        // } else {
        //     return View();
        // }
    }

    [HttpGet("{name}/Customer/{id}")]
    public IActionResult ShowCustomer(string name, int id)
    {
        Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

        MyViewModel MyModel = new MyViewModel
        {
            Business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
            Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId")),
            Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId")),
            Customer = _context.Customers.FirstOrDefault(e => e.CustomerId == id)
        };

        return View(MyModel);
    }


    [HttpPost("{name}/Team/Create")]
    public IActionResult CreateEmployee(string name, Employee newEmployee)
    {
        // if (!ModelState.IsValid)
        // {
        //     return View("NewEmployee", new MyViewModel
        //     {
        //         Business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(b => b.Name == name),
        //         Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
        //     });
        // }
        newEmployee.BusinessId = (int)HttpContext.Session.GetInt32("BusinessId");
        PasswordHasher<Employee> hasher = new PasswordHasher<Employee>();
        newEmployee.Password = hasher.HashPassword(newEmployee, newEmployee.Password);
        _context.Add(newEmployee);
        _context.SaveChanges();
        return RedirectToAction("Team", new { name });
    }


    [HttpGet("{name}/Sales")]
    public IActionResult Sales(string name)
    {
        if (HttpContext.Session.GetInt32("OwnerId") != null)
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
            };
            return View(MyModel);
        }
        else
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId"))
            };
            return View(MyModel);
        }
    }

    [HttpGet("{name}/Calendar")]
    public IActionResult Calendar(string name)
    {
        if (HttpContext.Session.GetInt32("OwnerId") != null)
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
            };
            return View(MyModel);
        }
        else
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).Include(e => e.Employees).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Employee = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.EmployeeId == HttpContext.Session.GetInt32("EmployeeId"))
            };
            return View(MyModel);
        }
    }

    [HttpPost("employee/login")]
    public IActionResult LoginUser(LoginUser loginUser)
    {
        if (ModelState.IsValid)
        {
            Employee? employeeInDb = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Email == loginUser.LEmail);
            Owner? ownerInDb = _context.Owners.FirstOrDefault(u => u.Email == loginUser.LEmail);

            if (ownerInDb != null)
            {

                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(loginUser, ownerInDb.Password, loginUser.LPassword);

                if (result == 0)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Login");
                }
                else
                {
                    HttpContext.Session.SetInt32("OwnerId", ownerInDb.OwnerId);
                    return RedirectToAction("BusinessSelect");
                }
            }
            else if (employeeInDb != null && ownerInDb == null)
            {
                Console.WriteLine(employeeInDb.Email, employeeInDb.FirstName);
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(loginUser, employeeInDb.Password, loginUser.LPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Login");
                }
                else
                {
                    HttpContext.Session.SetInt32("EmployeeId", employeeInDb.EmployeeId);
                    HttpContext.Session.SetInt32("BusinessId", employeeInDb.BusinessId);
                    return RedirectToAction("Dashboard", new { name = employeeInDb.Company.Name });
                }
            }
            else
            {
                Console.WriteLine("No employee :C");
                ModelState.AddModelError("LEmail", "Invalid Email");
                return View("Login");
            }
        }
        else
        {
            return View("Login");
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        int? ownerId = context.HttpContext.Session.GetInt32("OwnerId");
        if (userId == null && ownerId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
        else if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}