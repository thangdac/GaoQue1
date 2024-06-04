using GaoQue.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GaoQue.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class HomeManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeManagerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetOrderDataForChart()
        {
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 12, 31);

            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();

            var ordersByMonth = orders
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new { Month = g.Key, TotalAmount = g.Sum(o => o.TotalPrice) })
                .OrderBy(g => g.Month)
                .ToList();

            var labels = ordersByMonth.Select(g => DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(g.Month)).ToArray();
            var data = ordersByMonth.Select(g => g.TotalAmount).ToArray();

            return Json(new { Labels = labels, Data = data });
        }






    }
}
