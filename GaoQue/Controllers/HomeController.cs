using GaoQue.Models;
using GaoQue.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace GaoQue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() // Đặt phương thức thành bất đồng bộ
        {
            var Products = await _context.Products.ToListAsync();
            var Categories = await _context.Categories.ToListAsync();
            var Menus = await _context.Menus.ToListAsync();
            var Sliders = await _context.Sliders.ToListAsync();
            var Contacts = await _context.Contacts.ToListAsync();
            var Banners = await _context.Banners.ToListAsync();
            var Product = await _context.Products.Where(m => m.Id == 1).OrderBy(m => m.Order).Take(4).ToListAsync();
            var Category = await _context.Categories.Where(m => m.Id == 1).FirstOrDefaultAsync();
            var viewModel = new HomeViewModel
            {
                Products = Products,
                Categories = Categories, 
                Menus = Menus,    
                Sliders = Sliders,
                Contacts = Contacts, 
                Banners = Banners
            };
            return View(viewModel);
        }

        public async Task<IActionResult> _ProductPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _CategoryPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> _SliderPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _ContactPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> _BannerPartial()
        {
            return PartialView();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
