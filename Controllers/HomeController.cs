﻿using System.Diagnostics;
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
            Owner = _context.Owners.Include(e => e.CreatedBusinesses).FirstOrDefault(u => u.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
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


    [HttpGet("{name}/dashboard")]
    public IActionResult Dashboard(string name)
    {   
        if(HttpContext.Session.GetInt32("OwnerId") != null)
        {
            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Where(e => e.BusinessOwnerId == HttpContext.Session.GetInt32("OwnerId")).FirstOrDefault(e => e.Name == name),
                Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
            };
            if(MyModel.Business == null)
            {
                return RedirectToAction("BusinessSelect");
            }
            return View(MyModel);
        }
        else
        {
            Business? biz = _context.Businesses.FirstOrDefault(b => b.Name == name);

            MyViewModel MyModel = new MyViewModel
            {
                Business = _context.Businesses.Include(e => e.BusinessOwner).FirstOrDefault(i => i.BusinessId == biz.BusinessId),
                Owner = _context.Owners.FirstOrDefault(e => e.OwnerId == HttpContext.Session.GetInt32("OwnerId"))
            };
            return View(MyModel);
        }

    }

    [HttpPost("employee/login")]
    public IActionResult LoginUser(LoginUser loginUser)
    {
        if(ModelState.IsValid)
        {
            Owner? ownerInDb = _context.Owners.FirstOrDefault(u => u.Email == loginUser.LEmail);
            if(ownerInDb == null)
            {
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("Login");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(loginUser, ownerInDb.Password, loginUser.LPassword);

            if(result == 0)
            {
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("Login");
            } else {
                HttpContext.Session.SetInt32("OwnerId", ownerInDb.OwnerId);
                // HttpContext.Session.SetInt32("EmployeeId", ownerInDb.OwnerId);
                return RedirectToAction("BusinessSelect");
            }
        } else {
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
        if(userId == null && ownerId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        } else if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}