using GaoQue.DataAccess;
using GaoQue.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GaoQue.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UsersManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UsersManagerController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(userRolesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoles(string id)
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var model = new AddRoleViewModel { UserId = id, Roles = roles };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoles(AddRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                var role = new ApplicationRole { Name = model.RoleName };
                await _roleManager.CreateAsync(role);
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return RedirectToAction("Index"); // Thay thế bằng hành động phù hợp
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // Lấy lại danh sách các vai trò nếu có lỗi xảy ra và cần hiển thị lại form
            model.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return RedirectToAction("Index"); // Thay thế bằng hành động phù hợp
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // Nếu có lỗi, lấy lại thông tin người dùng và các vai trò
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userRolesViewModel.Add(new UserRolesViewModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Roles = roles.ToList()
                });
            }

            return View("Index", userRolesViewModel);
        }

    }
}

