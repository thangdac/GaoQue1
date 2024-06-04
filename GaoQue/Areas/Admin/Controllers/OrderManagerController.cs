using GaoQue.Models;
using GaoQue.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace GaoQue.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrderManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        //hien thi danh sach don hang
        public async Task<IActionResult> Index(int? page)
        {
            var orders = 
                await _context.Orders.Include(o => o.ApplicationUser).ToListAsync();
            return View(orders);
        }

        //hiên thi chi tiet don hang
        public async Task<IActionResult> Display(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = _context.Orders.Find(id);
            if (item != null)
            {
                _context.Orders.Attach(item);
                item.Status = trangthai;
                _context.Entry(item).Property(x => x.Status).IsModified = true;
                _context.SaveChanges();
                return Json(new { message = "Success", Success = true }); // Đảm bảo rằng Success được trả về và có giá trị true
            }
            return Json(new { message = "unsuccess", Success = false });
        }


    }
}
