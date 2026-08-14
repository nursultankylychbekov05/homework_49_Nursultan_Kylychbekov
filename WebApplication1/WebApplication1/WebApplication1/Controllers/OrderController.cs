using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class OrderController : Controller
{
    private MobileContext _context;

    public OrderController(MobileContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        List<Order> orders = _context.Orders.Include(o => o.Phone).ToList();
        return View(orders);
    }
    public IActionResult Create(int id)
    {
        Phone? p = _context.Phones.FirstOrDefault(p => p.Id == id);
        return View(new Order(){Phone = p});
    }

    [HttpPost]
    public IActionResult Create(Order? order)
    {
        if (order != null)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}