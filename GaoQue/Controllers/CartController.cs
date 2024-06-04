using GaoQue.DataAccess;
using GaoQue.Extensions;
using GaoQue.Models;
using GaoQue.Repositories;
using GaoQue.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GaoQue.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string CartSession = "CartSession";

        public CartController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IProductRepository
            productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var logins = await _userManager.Users.ToListAsync();
            var orders = await _context.Orders.ToListAsync(); // Lấy danh sách các đơn hàng
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var cart = HttpContext.Session.GetString(CartSession);
            var list = new List<CartItem>();

            if (!string.IsNullOrEmpty(cart))
            {
                list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
            }

            var viewModel = new CartViewModel
            {
                Menus = menus,
                Logins = logins,
                CartItems = list,
                Orders = orders,
                CurrentUser = currentUser
            };

            return View(viewModel);
        }

        public IActionResult AddItem(int ProductId, int Quantity)
        {
            var response = new { Success = false, Message = "Failed to add product." };

            var product = _context.Products.Find(ProductId);
            if (product == null)
            {
                response = new { Success = false, Message = "Product not found." };
                return Json(response);
            }

            var cart = HttpContext.Session.GetString(CartSession);
            List<CartItem> cartItems;

            if (!string.IsNullOrEmpty(cart))
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cart);
            }
            else
            {
                cartItems = new List<CartItem>();
            }

            var existingItem = cartItems.FirstOrDefault(x => x.Product.Id == ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += Quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    Product = product,
                    Quantity = Quantity
                };
                cartItems.Add(newItem);
            }

            HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(cartItems));
            response = new { Success = true, Message = "Product added successfully!" };

            return Json(response);
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

        public IActionResult DeleteAll()
        {
            HttpContext.Session.Remove(CartSession);
            return Json(new
            {
                status = true
            });
        }

        public IActionResult Delete(int id)
        {
            var sessionCart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(CartSession));
            sessionCart.RemoveAll(x => x.Product.Id == id);
            HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(sessionCart));
            return Json(new
            {
                status = true
            });
        }

        public IActionResult Update(string cartModel)
        {
            var jsonCart = JsonConvert.DeserializeObject<List<CartItem>>(cartModel);
            var sessionCart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(CartSession));
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(sessionCart));
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        public async Task<IActionResult> ProcessPayment(string name)
        {
            // Lấy dữ liệu cần thiết cho trang thanh toán
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var logins = await _userManager.Users.ToListAsync();
            var orders = await _context.Orders.ToListAsync(); // Lấy danh sách các đơn hàng
            var cart = HttpContext.Session.GetString(CartSession);
            var list = new List<CartItem>();

            if (!string.IsNullOrEmpty(cart))
            {
                list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
            }

            // Chuẩn bị ViewModel với dữ liệu đã lấy
            var cartViewModel = new CartViewModel
            {
                Menus = menus,
                Logins = logins,
                Orders = orders,
                CartItems = list
            };

            // Trả về view thanh toán
            return View(cartViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string ShippingAddress, string Notes, string paymentMethod)
        {
            // Lấy thông tin người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (currentUser == null)
            {
                // Nếu không đăng nhập, chuyển hướng người dùng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin sản phẩm từ giỏ hàng trong session
            var cart = HttpContext.Session.GetString(CartSession);
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cart);

            if (cartItems == null || !cartItems.Any())
            {
                // Nếu giỏ hàng trống, chuyển hướng đến trang thích hợp
                return RedirectToAction("Index", "Cart");
            }

            // Tính toán tổng giá trị đơn hàng
            decimal totalAmount = cartItems.Sum(item => item.Product.Price.GetValueOrDefault(0) * item.Quantity);

            // Tạo một đối tượng Order để lưu thông tin đơn hàng
            var order = new Order
            {
                UserId = currentUser.Id,
                OrderDate = DateTime.Now,
                Notes = Notes, 
                PaymentMethod = paymentMethod,
                ShippingAddress = ShippingAddress, 
                TotalPrice = totalAmount,
                Status = 1
            };

            // Thêm đơn hàng vào cơ sở dữ liệu
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Lưu thông tin chi tiết đơn hàng
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Price = item.Product.Price ?? 0 // Giả sử mỗi sản phẩm có một giá và sử dụng giá trị mặc định là 0 nếu Price là null
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();

            // Xóa giỏ hàng sau khi đã thanh toán
            HttpContext.Session.Remove(CartSession);

            // Chuyển hướng người dùng đến trang hoàn thành đơn hàng
            return RedirectToAction("Complete");
        }


        public async Task<IActionResult> Complete()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var logins = await _userManager.Users.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            // Filter the orders to get only the ones for the current user
            var orders = await _context.Orders.Where(o => o.UserId == currentUser.Id).OrderByDescending(o => o.OrderDate).ToListAsync();

            var recentOrder = orders.FirstOrDefault(); // Get the most recent order

            var cart = HttpContext.Session.GetString(CartSession);
            var list = new List<CartItem>();

            if (!string.IsNullOrEmpty(cart))
            {
                list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
            }

            var viewModel = new CartViewModel
            {
                Menus = menus,
                Logins = logins,
                CartItems = list,
                Orders = orders,
                CurrentUser = currentUser,
                RecentOrder = recentOrder // Add the most recent order to the view model
            };

            return View(viewModel);
        }

    }
}
