using GaoQue.DataAccess;
using GaoQue.Models;
using X.PagedList;
using System.Linq;
using GaoQue.Repositories;
using GaoQue.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaoQue.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context, IProductRepository productRepository
            , ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var logins = await _userManager.Users.ToListAsync();
            var products = await _productRepository.GetAllAsync();

            int pageSize = 8;
            int pageNumber = page ?? 1;

            var PagedProducts = await _context.Products.OrderBy(p => p.Name).ToPagedListAsync(pageNumber, pageSize);

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Categories = categories,
                Products = products,    
                PagedProducts = PagedProducts,
                Logins = logins
            };
            return View(viewModel);
        }


        //chi tiet san pham
        public async Task<IActionResult> ProductDetail(int id)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var productInformation = await _context.productInformation.Where(pi => pi.ProductId == id).ToListAsync();
            var logins = await _userManager.Users.ToListAsync();
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Product Error",
                };
                return View("Error", errorViewModel);
            }
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Informations = productInformation,
                Product = product,
                Logins = logins
            };
            return View(viewModel);
        }


        // Partial view cho menu
        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }

        // Partial view cho login
        public async Task<IActionResult> _LoginPartial()
        {
            return PartialView();
        }

        // Partial view cho information
        public async Task<IActionResult> _InformationPartial()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return BadRequest("Search query is required.");
                var resultProducts = await _context.Products.Where(
                    p => p.Name.Contains(query) ||
                    (p.Description != null && p.Description.Contains(query))).ToListAsync();

                // Lấy các dữ liệu khác cần thiết cho view
                var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
                var categories = await _categoryRepository.GetAllAsync();
                var logins = await _userManager.Users.ToListAsync();

                // Tạo đối tượng ProductViewModel
                var viewModel = new ProductViewModel
                {
                    Menus = menus,
                    Categories = categories,
                    Products = resultProducts,
                    Logins = logins
                };

                return View("SearchResults", viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
