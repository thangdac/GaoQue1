using GaoQue.Controllers;
using GaoQue.Models;
using GaoQue.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaoQue.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CategoryManagerController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryManagerController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var Category = await
                 _categoryRepository.GetAllAsync();
            return View(Category);
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var Category = await _categoryRepository.GetByIdAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await
                _categoryRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage

                    category.ImageUrl = await SaveImage(imageUrl);

                }
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            return View(category);
        }
        // Viết thêm hàm SaveImage (tham khảo bài 02)
        private async Task<string> SaveImage(IFormFile image)
        {
            //Thay đổi đường dẫn theo cấu hình của bạn
            var savePath = Path.Combine("wwwroot/img", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return image.FileName; // Trả về đường dẫn tương đối
        }

        // Hiển thị form cập nhật danh mục
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }
                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (imageUrl == null)
                {
                    category.ImageUrl = existingCategory.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới và cập nhật URL hình ảnh trong danh mục
                    category.ImageUrl = await SaveImage(imageUrl);
                }

                // Cập nhật các thông tin khác của danh mục
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.ImageUrl = category.ImageUrl;

                await _categoryRepository.UpdateAsync(existingCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Hiển thị form xác nhận xóa danh mục
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // Xử lý xóa danh mục
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
