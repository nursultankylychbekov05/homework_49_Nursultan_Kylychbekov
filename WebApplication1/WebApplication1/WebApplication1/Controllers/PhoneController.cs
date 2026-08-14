using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class PhoneController : Controller
{
    private readonly MobileContext _context;
    private readonly IWebHostEnvironment _env; 

    public PhoneController(MobileContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    
    public IActionResult Index()
    {
        List<Phone> phones = _context.Phones.ToList();
        return View(phones);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Phone? phone)
    {
        if (phone != null)
        {
            _context.Phones.Add(phone);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    
    public IActionResult DownloadSpecs(int id)
    {
        var phone = _context.Phones.FirstOrDefault(p => p.Id == id);
        if (phone == null) return NotFound();
        
        string fileName = $"{phone.Company}.txt";
        string filePath = Path.Combine(_env.WebRootPath, "files", fileName);
        
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        
        string clientDownloadName = $"{phone.Name}.txt";

        return File(fileBytes, "text/plain", clientDownloadName);
    }
    
    public IActionResult RedirectToCompany(int id)
    {
        var phone = _context.Phones.FirstOrDefault(p => p.Id == id);
        if (phone == null) return NotFound();
        
        string targetUrl = phone.Company.ToLower() switch
        {
            "apple" => "https://www.apple.com",
            "samsung" => "https://www.samsung.com",
            "xiaomi" => "https://www.mi.com",
            "google" => "https://store.google.com",
            "huawei" => "https://consumer.huawei.com",
            _ => $"https://www.google.com/search?q={Uri.EscapeDataString(phone.Company)}"
        };
        
        return Redirect(targetUrl);
    }
}