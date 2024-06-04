using GaoQue.DataAccess;
using GaoQue.Models;
using GaoQue.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaoQue.Controllers
{
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BannerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var logins = await _userManager.Users.ToListAsync();
            var viewModel = new BannerViewModel
            {
                Menus = menus,
                Logins = logins,
            };
            return View(viewModel);
        }

        // Action trả về dữ liệu JSON
        public async Task<IActionResult> GetData()
        {
            var data = await _context.GPTDatas.ToListAsync();
            return Json(data);
        }

        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _LoginPartial()
        {
            return PartialView();
        }
    }
}




